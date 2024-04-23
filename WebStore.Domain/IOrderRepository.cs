using FluentResults;

namespace WebStore.Domain
{
    public interface IOrderRepository
    {
        public Task<Result> CreateOrderAsync(Order client);
        public Task<Result> UpdateOrderAsync(Order client);
        public Task<Result> DeleteOrderAsync(int id);
        public Task<Result<Order>> GetOrderAsync(int id);
        public Task<Result<Order>> GetOrderByUniqueNameAsync(string name);
        public Task<Result<IEnumerable<Order>>> GetAllOrdersAsync();
        public Task<Result> IsOrderNameUniqueAsync(string name);
    }
}