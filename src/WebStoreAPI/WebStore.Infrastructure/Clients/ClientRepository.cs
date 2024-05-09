using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebStore.API.Domain;
using WebStore.EventBus.Events;

namespace WebStore.API.Infrastructure.Clients
{
    public class ClientRepository(WebStoreContext context) : IClientRepository
    {
        private readonly WebStoreContext _context = context;

        public async Task<Result> AddClientAsync(Client client)
        {
            try
            {
                await _context.Clients.AddAsync(client);
                await _context.ClientEvents.AddAsync(CreateClientEvent(client, "client_created"));

                await _context.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }

        public async Task<Result> UpdateClientAsync(Client client)
        {
            var entity = await _context.Clients.FindAsync(client.Id);

            if (entity is null)
            {
                return Result.Fail($"{client.Id} is not found");
            }

            try
            {
                entity.Orders.Clear();
                entity.Orders.AddRange(client.Orders);

                entity.Email = client.Email;
                entity.PhoneNumber = client.PhoneNumber;
                entity.Name = client.Name;

                await _context.ClientEvents.AddAsync(CreateClientEvent(client, "client_updated"));

                await _context.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }

        public async Task<Result<Client>> DeleteClientAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client is null)
            {
                return Result.Fail($"{id} is not found");
            }

            try
            {
                _context.Clients.Remove(client);
                await _context.ClientEvents.AddAsync(CreateClientEvent(client, "client_deleted"));
                await _context.SaveChangesAsync();

                return Result.Ok(client);
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }

        public async Task<Result<Client>> GetClientAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client is not null)
            {
                return Result.Ok(client);
            }

            return Result.Fail($"{id} is not found");
        }

        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber) => !await _context.Clients.AnyAsync(x => x.PhoneNumber.Equals(phoneNumber));
        public async Task<bool> IsEmailUniqueAsync(string email) => !await _context.Clients.AnyAsync(x => x.Email.Equals(email));
        public async Task<Result<IEnumerable<Client>>> GetAllClientsAsync() => Result.Ok(await _context.Clients.ToListAsync() as IEnumerable<Client>);

        private static ClientEvent CreateClientEvent(Client client, string queueName) => new(client.Id.ToString(), client.Name, client.PhoneNumber,
            client.Email, "users/players/customers", queueName, JsonSerializer.Serialize(client.Orders));
    }
}