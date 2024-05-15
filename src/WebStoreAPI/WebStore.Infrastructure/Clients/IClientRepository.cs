using FluentResults;
using WebStore.API.Domain;

namespace WebStore.API.Infrastructure.Clients
{
    public interface IClientRepository
    {
        public Task AddClientAsync(Client client);
        public Task<Result> UpdateClientAsync(Client client);
        public Task<Result<Client>> DeleteClientAsync(Guid id);
        public Task<Result<Client>> GetClientAsync(Guid id);
        public Result<IEnumerable<Client>> GetPaginatedClients(int page);
        public Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber);
        public Task<bool> IsEmailUniqueAsync(string email);
    }
}