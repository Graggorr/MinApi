using FluentResults;

namespace WebStore.Domain
{
    public interface IClientRepository
    {
        public Task<bool> AddClientAsync(Client client);
        public Task<RepositoryResult> UpdateClientAsync(Client client);
        public Task<RepositoryResult> DeleteClientAsync(Guid id);
        public Task<Result<Client>> GetClientAsync(Guid id);
        public Task<Result<IEnumerable<Client>>> GetAllClientsAsync();
        public Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber);
        public Task<bool> IsEmailUniqueAsync(string email);
    }
}