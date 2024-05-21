using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WebStore.API.Domain;
using WebStore.Events.Orders;
using WebStore.Extensions;

namespace WebStore.API.Infrastructure.Orders
{
    public class OrderRepository(WebStoreContext context, ILogger<IOrderRepository> logger) : IOrderRepository
    {
        private readonly WebStoreContext _context = context;
        private readonly ILogger _logger = logger;

        public async Task<int> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            var client = await _context.Clients.FindAsync(order.ClientId);
            client.Orders.Add(order);
            _context.OrderEvents.Add(CreateOrderEvent(order, "order_created"));

            await _context.SaveChangesAsync();

            _logger.LogInformation($"A new order has been added:\n{JsonSerializer.Serialize(order)}");

            return order.Id;
        }

        public async Task<Result> UpdateOrderAsync(Order order)
        {
            var orderEntity = await _context.Orders.FindAsync(order.Id);

            if (orderEntity is null)
            {
                var message = $"Order ({order.Id}) is not found";

                return _logger.LogSendAndFail(message);
            }

            orderEntity.Price = order.Price;
            orderEntity.Description = order.Description;
            orderEntity.Name = order.Name;

            _context.OrderEvents.Add(CreateOrderEvent(order, "order_updated"));
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order ({orderEntity.Id}) has been updated. New statement:\n{JsonSerializer.Serialize(orderEntity)}");

            return Result.Ok();
        }

        public async Task<Result<Order>> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null)
            {
                var message = $"Order ({id}) is not found";

                return _logger.LogSendAndFail(message);
            }

            _context.Orders.Remove(order);

            var client = await _context.Clients.FindAsync(order.ClientId);
            client.Orders.Remove(order);

            _context.OrderEvents.Add(CreateOrderEvent(order, "order_deleted"));
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order ({id}) has been deleted:\n{JsonSerializer.Serialize(order)}");

            return Result.Ok(order);
        }

        public async Task<Result<Order>> GetOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null)
            {
                var message = $"{id} is not found";

                return _logger.LogSendAndFail(message);
            }

            return Result.Ok(order);
        }

        public async Task<Result<IEnumerable<Order>>> GetClientOrdersAsync(Guid clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);

            if (client is null)
            {
                var message = $"Client with {clientId} is not found";

                return _logger.LogSendAndFail(message);
            }

            return Result.Ok(client.Orders as IEnumerable<Order>);
        }

        private static OrderEvent CreateOrderEvent(Order order, string queueName) => new(order.Id.ToString(), order.ClientId.ToString(),
            order.Name, order.Description, order.Price, "users/players/customers/orders", queueName);        
    }
}