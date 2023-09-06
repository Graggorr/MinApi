namespace Interactions.Database.Entities
{
    public class BookEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public List<AuthorEntity> Authors { get; set; }
    }
}
