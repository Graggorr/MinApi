﻿using FluentResults;
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

        public async Task<RepositoryResult> UpdateClientAsync(Client client)
        {
            var entity = await _context.Clients.SingleOrDefaultAsync(x => x.Id.Equals(client.Id));

            if (entity is null)
            {
                return RepositoryResult.NotFound;
            }

            entity.Orders = client.Orders;
            entity.Email = client.Email;
            entity.Name = client.Name;

            await _context.SaveChangesAsync();

            return RepositoryResult.Success;
        }

        public async Task<RepositoryResult> DeleteClientAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client is null)
            {
                return RepositoryResult.NotFound;
            }

            var entityEntry = _context.Clients.Remove(client);

            if (entityEntry.State is EntityState.Deleted)
            {
                await _context.SaveChangesAsync();

                return RepositoryResult.Success;
            }

            return RepositoryResult.Failed;
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

        public async Task<bool> IsNumberUniqueAsync(string phoneNumber) => !await _context.Clients.AnyAsync(x => x.Equals(phoneNumber));
        public async Task<bool> IsEmailUniqueAsync(string email) => !await _context.Clients.AnyAsync(x => x.Equals(email));

        public async Task<Result<IEnumerable<Client>>> GetAllClientsAsync() => Result.Ok(await _context.Clients.ToListAsync() as IEnumerable<Client>);
    }
}