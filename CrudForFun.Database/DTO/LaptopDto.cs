using WebStore.Database.DTO;

namespace WebStore.Infrastructure.DTO
{
    public class LaptopDto: ItemDto
    {
        public string Producer { get; set; }
        public string Country { get; set; }
    }
}
