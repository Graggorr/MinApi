using Interactions.Database.Dtos;

namespace Interactions.UnitTests
{
    [TestClass]
    public class BookDomainTests
    {
        private readonly BookDto _dto1;
        private readonly BookDto _dto2;

        public BookDomainTests()
        {
            _dto1 = new BookDto
            {
                Name = "string",
                Description = "string",
                Authors = new List<string>
                {
                    "string"
                },
            };
            _dto2 = new BookDto
            {
                Name = "string",
                Description = "string1",
                Authors = new List<string>
                {
                    "string1"
                },
            };
        }

        [TestMethod]
        public async Task AddBook()
        {
            var domain = Utils.CreateBookDomain();
            var result = await domain.Add(new BookDto[] { _dto1 });

            Assert.IsTrue(result.StatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GetBook()
        {
            var domain = Utils.CreateBookDomain(true);
            await domain.Add(new BookDto[] { _dto1 });
            var newDomain = Utils.CreateBookDomain();
            var result = await newDomain.Get(1);

            Assert.AreEqual(_dto1, result);
        }

        [TestMethod]
        public async Task RemoveBook()
        {
            var domain = Utils.CreateBookDomain(true);
            await domain.Add(new BookDto[] { _dto1 });
            var newDomain = Utils.CreateBookDomain();
            var result = await newDomain.Remove(_dto1);

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task UpdateBook()
        {
            var domain = Utils.CreateBookDomain(true);
            await domain.Add(new BookDto[] { _dto1 });
            var newDomain = Utils.CreateBookDomain();
            var result = await newDomain.Update(_dto2);

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetAll()
        {
            var domain = Utils.CreateBookDomain(true);
            var array = new BookDto[] { _dto1, _dto2 };
            await domain.Add(array);
            var newDomain = Utils.CreateBookDomain();
            var result = (List<BookDto>)await newDomain.GetAll();

            Assert.AreEqual(array.Length, result.Count());
            Assert.AreEqual(result[0], array[0]);
            Assert.AreEqual(result[1], array[2]);
        }
    }
}