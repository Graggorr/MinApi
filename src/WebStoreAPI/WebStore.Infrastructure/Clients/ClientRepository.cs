using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WebStore.API.Domain;
using WebStore.EventBus.Events;

namespace WebStore.API.Infrastructure.Clients
{
    public class ClientRepository(WebStoreContext context, ILogger<IClientRepository> logger) : IClientRepository
    {
        private readonly WebStoreContext _context = context;
        private readonly ILogger _logger = logger;

        public async Task AddClientAsync(Client client)
        {
            _context.Clients.Add(client);
            _context.ClientEvents.Add(CreateClientEvent(client, "client_created"));

            await _context.SaveChangesAsync();

            _logger.LogInformation($"A new client has been added:\n{JsonSerializer.Serialize(client)}");
        }

        public async Task<Result> UpdateClientAsync(Client client)
        {
            var entity = await _context.Clients.FindAsync(client.Id);

            if (entity is null)
            {
                var message = $"Client ({client.Id}) is not found";

                return LogAndSendFail(message);
            }

            entity.Orders.Clear();

            entity.Email = client.Email;
            entity.PhoneNumber = client.PhoneNumber;
            entity.Name = client.Name;
            entity.Orders.AddRange(client.Orders);

            await _context.ClientEvents.AddAsync(CreateClientEvent(client, "client_updated"));
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Client ({entity.Id}) has been updated. New statement:\n{JsonSerializer.Serialize(entity)}");

            return Result.Ok();
        }

        public async Task<Result<Client>> DeleteClientAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client is null)
            {
                var message = $"Client ({id}) is not found";

                return LogAndSendFail(message);
            }

            _context.Clients.Remove(client);

            await _context.ClientEvents.AddAsync(CreateClientEvent(client, "client_deleted"));
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Client ({id}) has been deleted:\n{JsonSerializer.Serialize(client)}");

            return Result.Ok(client);
        }

        public async Task<Result<Client>> GetClientAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client is null)
            {
                var message = $"{id} is not found";

                return LogAndSendFail(message);
            }

            return Result.Ok(client);
        }

        public Result<IEnumerable<Client>> GetPaginatedClients(int page)
        {
            const int pageSize = 25;
            var clients = _context.Clients.Skip(page * pageSize).Take(pageSize);

            return Result.Ok(clients.AsEnumerable());
        }

        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber)
            => !await _context.Clients.AnyAsync(x => x.PhoneNumber.Equals(phoneNumber));
        public async Task<bool> IsEmailUniqueAsync(string email)
            => !await _context.Clients.AnyAsync(x => x.Email.Equals(email));

        private static ClientEvent CreateClientEvent(Client client, string queueName) => new(client.Id.ToString(), client.Name, client.PhoneNumber,
            client.Email, "users/players/customers", queueName, JsonSerializer.Serialize(client.Orders));

        private Result LogAndSendFail(string message)
        {
            _logger.LogDebug(message);

            return Result.Fail(message);
        }
    }
}