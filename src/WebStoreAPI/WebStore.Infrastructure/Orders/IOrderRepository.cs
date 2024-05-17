using FluentResults;
using WebStore.API.Domain;

namespace WebStore.API.Infrastructure.Orders
{
    public interface IOrderRepository
    {
        public Task AddOrderAsync(Order order);
        public Task<Result> UpdateOrderAsync(Order order);
        public Task<Result<Order>> DeleteOrderAsync(int id);
        public Task<Result<Order>> GetOrderAsync(int id);
        public Task<Result<IEnumerable<Order>>> GetClientOrdersAsync(Guid clientId);
        public Task<bool> IsClientExist(Guid clientId);
    }
}