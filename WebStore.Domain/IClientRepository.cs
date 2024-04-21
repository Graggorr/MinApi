using FluentResults;

namespace WebStore.Domain
{
    public interface IClientRepository
    {
        public Task<Result> PostClientAsync(Client client);
        public Task<Result> PutClientAsync(Client client);
        public Task<Result> DeleteClientAsync(string phoneNumber);
        public Task<Result<Client>> GetClientAsync(string phoneNumber);
        public Task<Result<IEnumerable<Client>>> GetAllClientsAsync();
        public Task<Result> IsNumberUniqueAsync(string phoneNumber);
        public Task<Result> IsEmailUniqueAsync(string email);
    }
}