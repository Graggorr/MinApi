using Interactions.Database.Dtos;
using Mapster;

namespace Interactions.Domain.Common
{
    public interface IDomain<T> where T : ItemDto
    {
        public Task<HttpResponseMessage> Add(T[] items);
        public Task<HttpResponseMessage> Remove(T item);
        public Task<T> Get(int id);
        public Task<IEnumerable<T>> GetAll();
        public Task<HttpResponseMessage> Update(T item);
    }
}
