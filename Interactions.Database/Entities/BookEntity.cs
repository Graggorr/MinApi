using Interactions.Database.Dtos;
using Mapster;

namespace Interactions.Database.Entities
{
    public class BookEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public virtual List<AuthorEntity> Authors { get; set; }

        public BookDto Map()
        {
            var dto = this.Adapt<BookDto>();
            dto.Authors = new List<string>();

            foreach (var author in Authors)
            {
                dto.Authors.Add(author.Name);
            }

            return dto;
        }
    }
}
