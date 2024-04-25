using FluentResults;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain;

namespace WebStore.Infrastructure.Clients
{
    public class ClientRepository(WebStoreContext context) : IClientRepository
    {
        private readonly WebStoreContext _context = context;

        public async Task<bool> AddClientAsync(Client client)
        {
            var result = await _context.Clients.AddAsync(client);

            if (result.State is EntityState.Added)
            {
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateClientAsync(Client client)
        {
            var entity = await _context.Clients.FindAsync(client.Id);

            if (entity is null)
            {
                return false;
            }

            entity.Orders.Clear();
            entity.Orders.ForEach(entity.Orders.Add);
            entity.Email = client.Email;
            entity.Name = client.Name;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteClientAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client is null)
            {
                return false;
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Result<Client>> GetClientAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client is not null)
            {
                return Result.Ok(client);
            }

            return Result.Fail(new Error($"{id} is not contained in the repository"));
        }

        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber) => !await _context.Clients.AnyAsync(x => x.PhoneNumber.Equals(phoneNumber));
        public async Task<bool> IsEmailUniqueAsync(string email) => !await _context.Clients.AnyAsync(x => x.Email.Equals(email));

        public async Task<Result<IEnumerable<Client>>> GetAllClientsAsync() => Result.Ok(await _context.Clients.ToListAsync() as IEnumerable<Client>);
    }
}