namespace Interactions.Database.Entities
{
    public class AuthorEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<BookEntity> Books { get; set; }
    }
}
