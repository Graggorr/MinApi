using FluentResults;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public List<Client> Clients { get; set; }

        public static async Task<Result<Order>> CreateOrderAsync(OrderSwaggerDto dto)
        {
            return await Task.Factory.StartNew(() =>
            {
                return Result.Ok(new Order
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    Description = dto.Description,
                    Clients = null
                });
            });
        }

        public string ToStringEmailMessage() => $"\n{Name} - {Price}";
        public string ToStringWithoutClients() => $"id: {Id}\nname: {Name}\nprice: {Price}\ndescription: {Description}";
    }
}
