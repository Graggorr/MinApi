namespace Interactions.Database.Entities
{
    public class BookEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public virtual AuthorEntity Author { get; set; }
    }
}
