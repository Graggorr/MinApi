using FluentResults;

namespace WebStore.Domain
{
    public interface IOrderRepository
    {
        public Task<Result> PostOrderAsync(Order client);
        public Task<Result> PutOrderAsync(Order client);
        public Task<Result> DeleteOrderAsync(int id);
        public Task<Result<Order>> GetOrderAsync(int id);
        public Task<Result<Order>> GetOrderByUniqueNameAsync(string name);
        public Task<Result<IEnumerable<Order>>> GetAllOrdersAsync();
        public Task<Result> IsOrderNameUniqueAsync(string name);
    }
}