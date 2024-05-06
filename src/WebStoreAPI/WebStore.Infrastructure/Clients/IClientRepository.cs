using FluentResults;
using WebStore.Domain;

namespace WebStore.Infrastructure.Clients
{
    public interface IClientRepository
    {
        public Task<Result> AddClientAsync(Client client);
        public Task<Result> UpdateClientAsync(Client client);
        public Task<Result<Client>> DeleteClientAsync(Guid id);
        public Task<Result<Client>> GetClientAsync(Guid id);
        public Task<Result<IEnumerable<Client>>> GetAllClientsAsync();
        public Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber);
        public Task<bool> IsEmailUniqueAsync(string email);
    }
}