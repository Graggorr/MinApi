using FluentResults;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain;

namespace WebStore.Infrastructure.Clients
{
    public class ClientRepository(WebStoreContext context) : IClientRepository
    {
        private readonly WebStoreContext _context = context;

        public async Task<Result> PostClientAsync(Client client)
        {
            var result = await _context.Clients.AddAsync(client);

            if (result.State is EntityState.Added)
            {
                await _context.SaveChangesAsync();

                return Result.Ok();
            }

            return Result.Fail(new Error($"Cannot save {client.Name} into the repository"));
        }

        public async Task<Result> PutClientAsync(Client client)
        {
            var entity = await _context.Clients.SingleOrDefaultAsync(x => x.PhoneNumber.Equals(client.PhoneNumber));

            if (entity is null)
            {
                return Result.Fail(new Error($"{client.PhoneNumber} is not contained in the repository"));
            }

            entity.Orders = client.Orders;
            entity.Email = client.Email;
            entity.Name = client.Name;

            await _context.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> DeleteClientAsync(string phoneNumber)
        {
            var result = await GetClientAsync(phoneNumber);

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            var entityEntry = _context.Clients.Remove(result.Value);

            if (entityEntry.State is EntityState.Deleted)
            {
                await _context.SaveChangesAsync();

                return Result.Ok();
            }

            return Result.Fail(new Error($"Cannot remove {result.Value.Name} from the repository"));
        }

        public async Task<Result<Client>> GetClientAsync(string phoneNumber)
        {
            var client = await _context.Clients.FindAsync(phoneNumber);

            if (client is not null)
            {
                return Result.Ok(client);
            }

            return Result.Fail(new Error($"{phoneNumber} is not contained in the repository"));
        }

        public async Task<Result> IsNumberUniqueAsync(string phoneNumber) => await _context.Clients.AnyAsync(x => x.Equals(phoneNumber)) ?
             Result.Fail(new Error($"{phoneNumber} is already used")) : Result.Ok();

        public async Task<Result> IsEmailUniqueAsync(string email) => await _context.Clients.AnyAsync(x => x.Equals(email)) ?
            Result.Fail(new Error($"{email} is already used")) : Result.Ok();

        public async Task<Result<IEnumerable<Client>>> GetAllClientsAsync() => Result.Ok(await _context.Clients.ToListAsync() as IEnumerable<Client>);
    }
}