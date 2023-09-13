using Interactions.Database.Dtos;

namespace Interactions.UnitTests
{
    [TestClass]
    public class AuthorDomainTests
    {
        private readonly BookDto _bookDto;
        private readonly AuthorDto _authorDto;

        public AuthorDomainTests()
        {
            _bookDto = new BookDto
            {
                Name = "string",
                Description = "string",
                Authors = new List<string>
                {
                    "string"
                },
            };
            _authorDto = new AuthorDto
            {
                Name = "string",
                Books = new List<BookDto>()
                {
                    _bookDto
                }
            };
        }

        [TestMethod]
        public async Task GetAuthor()
        {
            var domain = Utils.CreateBookDomain(true);
            await domain.Add(new BookDto[] { _bookDto });
            var newDomain = Utils.CreateAuthorDomain();
            var result = await newDomain.Get(1);

            Assert.AreEqual(_bookDto, result);
        }

        [TestMethod]
        public async Task RemoveAuthor()
        {
            var domain = Utils.CreateBookDomain(true);
            await domain.Add(new BookDto[] {_bookDto});
            var newDomain = Utils.CreateAuthorDomain();
            var result = await newDomain.Remove(_authorDto);

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task UpdateAuthor()
        {
            var domain = Utils.CreateBookDomain(true);
            var newDomain = Utils.CreateAuthorDomain();
            await domain.Add(new BookDto[] { _bookDto });
            var result = await newDomain.Update(_authorDto);

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetAll()
        {
            var domain = Utils.CreateBookDomain(true);
            await domain.Add(new BookDto[] { _bookDto });
            var newDomain = Utils.CreateAuthorDomain();
            var result = (List<AuthorDto>)await newDomain.GetAll();

            Assert.AreEqual(_authorDto, result[0]);
        }
    }
}