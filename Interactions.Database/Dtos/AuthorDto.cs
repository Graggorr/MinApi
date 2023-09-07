namespace Interactions.Database.Dtos
{
    public class AuthorDto: ItemDto
    {
        public List<BookDto> Books { get; set; }
    }
}
