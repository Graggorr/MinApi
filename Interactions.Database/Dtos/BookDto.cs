namespace Interactions.Database.Dtos
{
    public class BookDto: ItemDto
    {
        public string Description { get; set; }
        public List<string> Authors { get; set; }
    }
}
