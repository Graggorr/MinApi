using System.ComponentModel.DataAnnotations;

namespace WebStore.Database.Entities
{
    public class PhoneEntity
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
