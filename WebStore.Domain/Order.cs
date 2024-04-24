using FluentResults;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain
{
    public class Order
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; init; }
        [Required]
        public double Price { get; init; }
        public string Description { get; init; }
        public IList<Client> Clients { get; init; }

        public static Task<Result<Order>> CreateOrderAsync(string name, double price, string description)
        {
            throw new NotImplementedException();
        }

        public string ToStringEmailMessage() => $"\n{Name} - {Price}";
        public string ToStringWithoutClients() => $"id: {Id}\nname: {Name}\nprice: {Price}\ndescription: {Description}";
    }
}
