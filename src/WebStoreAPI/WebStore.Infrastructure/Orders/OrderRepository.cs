using FluentResults;
using Microsoft.EntityFrameworkCore;
using WebStore.API.Domain;
using WebStore.API.Infrastructure;

namespace WebStore.API.Infrastructure.Orders
{
    public class OrderRepository(WebStoreContext context) : IOrderRepository
    {
        private readonly WebStoreContext _context = context;

        public async Task<bool> AddOrderAsync(Order Order)
        {
            var result = await _context.Orders.AddAsync(Order);

            if (result.State is EntityState.Added)
            {
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateOrderAsync(Order Order)
        {
            var entity = await _context.Orders.FindAsync(Order.Id);

            if (entity is null)
            {
                return false;
            }

            entity.Clients.Clear();
            entity.Clients.ForEach(entity.Clients.Add);
            entity.Description = Order.Description;
            entity.Name = Order.Name;
            entity.Price = Order.Price;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var Order = await _context.Orders.FindAsync(id);

            if (Order is null)
            {
                return false;
            }

            _context.Orders.Remove(Order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Result<Order>> GetOrderAsync(int id)
        {
            var Order = await _context.Orders.FindAsync(id);

            if (Order is not null)
            {
                return Result.Ok(Order);
            }

            return Result.Fail(new Error($"{id} is not contained in the repository"));
        }

        public async Task<Result<IEnumerable<Order>>> GetAllOrdersAsync() => Result.Ok(await _context.Orders.ToListAsync() as IEnumerable<Order>);
    }
}