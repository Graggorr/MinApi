using System.ComponentModel.DataAnnotations;
using WebStore.Infrastructure.Entities;

namespace WebStore.Database.Entities
{
    public class ItemEntity
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public required string Name { get; set; }
        public virtual PhoneEntity? Phone { get; set; }
        public virtual LaptopEntity? Laptop { get; set; }
    }
}
