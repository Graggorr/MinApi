using FluentResults;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain;

namespace WebStore.Infrastructure.Orders
{
    public class OrderRepository(WebStoreContext context) : IOrderRepository
    {
        public Task<Result> DeleteOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Order>>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Order>> GetOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Order>> GetOrderByUniqueNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Result> IsOrderNameUniqueAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CreateOrderAsync(Order client)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateOrderAsync(Order client)
        {
            throw new NotImplementedException();
        }
    }
}
