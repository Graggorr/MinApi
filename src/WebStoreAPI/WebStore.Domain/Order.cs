using FluentResults;
using System.ComponentModel.DataAnnotations;

namespace WebStore.API.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public List<Client> Clients { get; set; }

        public string ToStringEmailMessage() => $"\n{Name} - {Price}";
        public string ToStringWithoutClients() => $"id: {Id}\nname: {Name}\nprice: {Price}\ndescription: {Description}";
    }
}
