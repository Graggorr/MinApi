using FluentResults;

namespace WebStore.Domain
{
    public interface IOrderRepository
    {
        public Task<bool> AddOrderAsync(Order Order);
        public Task<bool> UpdateOrderAsync(Order Order);
        public Task<bool> DeleteOrderAsync(int id);
        public Task<Result<Order>> GetOrderAsync(int id);
        public Task<Result<IEnumerable<Order>>> GetAllOrdersAsync();
    }
}