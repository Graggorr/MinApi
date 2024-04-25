using FluentResults;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebStore.Domain;
using WebStore.Infrastructure;
using WebStore.Infrastructure.Clients;

namespace WebStore.InfrastructureUnitTests
{
    public class ClientRepositoryUnitTests
    {
        private WebStoreContext _context;
        private DbContextOptions<WebStoreContext> _options;

        private IClientRepository _clientRepository;
        private IOrderRepository _orderRepository;

        private Client _clientToAdd;
        private Client _clientToUpdateSuccess;
        private Client _clientToUpdateFail;
        private Client _clientToGet;
        private Client _clientToDelete;
        private Client _clientToTestUniqueValue;

        private Order _order1;
        private Order _order2;

        private ClientDto _clientToAddDto;
        private ClientDto _clientToUpdateSuccessDto;
        private ClientDto _clientToUpdateFailDto;
        private ClientDto _clientToGetDto;
        private ClientDto _clientToDeleteDto;
        private ClientDto _clientToTestUniqueValueDto;

        private OrderDto _orderDto1;
        private OrderDto _orderDto2;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<WebStoreContext>().UseInMemoryDatabase("webstore_db_in_memory").Options;
            _context = new WebStoreContext(_options, true);
            _clientRepository = new ClientRepository(_context);

            _orderDto1 = new OrderDto(1, "Name1", "description", 2.99, null);
            _orderDto2 = new OrderDto(2, "Name2", "description", 3.99, null);

            _order1 = Order.CreateOrderAsync(_orderDto1, _clientRepository).GetAwaiter().GetResult().Value;
            _order2 = Order.CreateOrderAsync(_orderDto2, _clientRepository).GetAwaiter().GetResult().Value;

            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetOrderAsync(1)).Returns(Task.FromResult(Result.Ok(_order1)));
            mock.Setup(m => m.GetOrderAsync(2)).Returns(Task.FromResult(Result.Ok(_order2)));
            _orderRepository = mock.Object;

            var guid = Guid.NewGuid();

            _clientToAddDto = new ClientDto(guid, "Add", "1111111111", "emailtoadd@gmail.com", new List<OrderDto> { _orderDto1 });
            _clientToUpdateSuccessDto = new ClientDto(guid, "Update", "222222222", "emailtoupdate@gmail.com", new List<OrderDto> { _orderDto2 });
            _clientToUpdateFailDto = new ClientDto(Guid.NewGuid(), "Update", "222222222", "emailtoupdate@gmail.com", new List<OrderDto> { _orderDto2 });
            _clientToGetDto = new ClientDto(Guid.NewGuid(), "Get", "3333333333", "emailtoget@gmail.com", new List<OrderDto> { _orderDto1 });
            _clientToDeleteDto = new ClientDto(Guid.NewGuid(), "Remove", "4444444444", "emailtoremove@gmail.com", new List<OrderDto> { _orderDto1 });
            _clientToTestUniqueValueDto = new ClientDto(Guid.NewGuid(), "Unique", "5555555555", "uniqueemail@gmail.com", new List<OrderDto> { _orderDto1 });

            _clientToAdd = Client.CreateClientAsync(_clientToAddDto, _clientRepository, _orderRepository, false, false).GetAwaiter().GetResult().Value;
            _clientToUpdateSuccess = Client.CreateClientAsync(_clientToUpdateSuccessDto, _clientRepository, _orderRepository, false, false).GetAwaiter().GetResult().Value;
            _clientToUpdateFail = Client.CreateClientAsync(_clientToUpdateFailDto, _clientRepository, _orderRepository, false, false).GetAwaiter().GetResult().Value;
            _clientToGet = Client.CreateClientAsync(_clientToGetDto, _clientRepository, _orderRepository, false, false).GetAwaiter().GetResult().Value;
            _clientToDelete = Client.CreateClientAsync(_clientToDeleteDto, _clientRepository, _orderRepository, false, false).GetAwaiter().GetResult().Value;
            _clientToTestUniqueValue = Client.CreateClientAsync(_clientToTestUniqueValueDto, _clientRepository, _orderRepository, false, false).GetAwaiter().GetResult().Value;
        }

        [Test]
        public async Task IsEmailUniqueTestPass()
        {
            var result = await _clientRepository.IsEmailUniqueAsync(_clientToTestUniqueValue.Email);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsEmailUniqueTestFail()
        {
            await _clientRepository.AddClientAsync(_clientToTestUniqueValue);
            var result = await _clientRepository.IsEmailUniqueAsync(_clientToTestUniqueValue.Email);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsPhoneNumberUniqueTestPass()
        {
            var result = await _clientRepository.IsPhoneNumberUniqueAsync(_clientToTestUniqueValue.PhoneNumber);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsPhoneNumberUniqueTestFail()
        {
            await _clientRepository.AddClientAsync(_clientToTestUniqueValue);
            var result = await _clientRepository.IsPhoneNumberUniqueAsync(_clientToTestUniqueValue.PhoneNumber);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddAsyncTestPass()
        {
            var result = await _clientRepository.AddClientAsync(_clientToAdd);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAsyncTestPass()
        {
            await _clientRepository.AddClientAsync(_clientToAdd);
            await _clientRepository.AddClientAsync(_clientToGet);

            var result = await _clientRepository.GetClientAsync(_clientToGet.Id);

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async Task GetAsyncTestFail()
        {
            await _clientRepository.AddClientAsync(_clientToAdd);

            var result = await _clientRepository.GetClientAsync(_clientToGet.Id);

            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public async Task DeleteAsyncTestPass()
        {
            await _clientRepository.AddClientAsync(_clientToAdd);
            await _clientRepository.AddClientAsync(_clientToDelete);

            var result = await _clientRepository.DeleteClientAsync(_clientToDelete.Id);

            Assert.IsTrue(result is RepositoryResult.Success);
        }

        [Test]
        public async Task DeleteAsyncTestNotFound()
        {
            await _clientRepository.AddClientAsync(_clientToAdd);

            var result = await _clientRepository.DeleteClientAsync(_clientToDelete.Id);

            Assert.IsTrue(result is RepositoryResult.NotFound);
        }

        [Test]
        public async Task UpdateAsyncTestPass()
        {
            await _clientRepository.AddClientAsync(_clientToAdd);
            var result = await _clientRepository.UpdateClientAsync(_clientToUpdateSuccess);

            Assert.IsTrue(result is RepositoryResult.Success);
        }

        [Test]
        public async Task UpdateAsyncTestNotFound()
        {
            await _clientRepository.AddClientAsync(_clientToAdd);

            var result = await _clientRepository.UpdateClientAsync(_clientToUpdateFail);

            Assert.IsTrue(result is RepositoryResult.NotFound);
        }
    }
}