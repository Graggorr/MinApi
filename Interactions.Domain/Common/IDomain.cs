using Interactions.Database.Dtos;
using Mapster;

namespace Interactions.Domain.Common
{
    public interface IDomain<T> where T : ItemDto
    {
        public Task<HttpResponseMessage> Add(T item);
        public Task<HttpResponseMessage> Remove(T item);
        public Task<T> Get(int id);
        public Task<IEnumerable<T>> GetAll();
        public Task<HttpResponseMessage> Update(T item);

        internal static IEnumerable<T> Map(IEnumerable<object> mappers)
        {
            var collection = new List<T>();

            try
            {
                foreach (var item in mappers)
                {
                    var mapped = item.Adapt<T>();
                    collection.Add(mapped);
                }
            }
            catch
            {
                collection = null;
            }

            return collection;
        }
    }
}
