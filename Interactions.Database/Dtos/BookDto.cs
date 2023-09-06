namespace Interactions.Database.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AuthorDto> Authors { get; set; }
    }
}
