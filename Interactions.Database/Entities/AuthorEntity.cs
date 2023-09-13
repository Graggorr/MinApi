using Interactions.Database.Dtos;
using Mapster;

namespace Interactions.Database.Entities
{
    public class AuthorEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int FeaturedBookId { get; set; }

        public virtual List<BookEntity> Books { get; set; }
        public virtual BookEntity FeaturedBook { get; set; }

        public AuthorDto Map()
        {
            var dto = this.Adapt<AuthorDto>();
            dto.Books = new List<BookDto>();

            foreach (var book in Books)
            {
                dto.Books.Add(book.Map());
            }

            return dto;
        }
    }
}
