using System.ComponentModel.DataAnnotations;
using WebStore.Database.Entities;

namespace WebStore.Infrastructure.Entities
{
    public class LaptopEntity
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Producer { get; set; }
        [Required]
        public required string Country { get; set; }
        public required ItemEntity Item { get; set; }
    }
}