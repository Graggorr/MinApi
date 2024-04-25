using FluentResults;
using Riok.Mapperly.Abstractions;
using WebStore.Domain;
using MapsterMapper;
using WebStore.EventBus.Common;
using WebStore.EventBus.Events;

namespace WebStore.Application.Clients;

public class PostClientRequestHandler(IClientRepository clientRepository, IOrderRepository orderRepository,
    IEventBus eventBus, ClientMapper mapper) : IPostClientRequestHandler
{
    //CQRS
    private readonly IClientRepository _repository = clientRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IEventBus _eventBus = eventBus;
    private readonly ClientMapper _mapper = mapper;

    public async Task<Result<Client>> Handle(ClientDto dto, CancellationToken cancellationToken)
    {
        var clientResult = await Client.CreateClientAsync(dto, _repository, _orderRepository, true, true);

        if (clientResult.IsFailed)
        {
            return Result.Fail(clientResult.Errors);
        }

        var client = clientResult.Value;

        var result = await _repository.AddClientAsync(client);

        if (result)
        {
            var integrationEvent = new ClientCreatedEvent(client.Id.ToString(), client.Name, client.PhoneNumber,
                client.Email, client.Orders.Select(x => x.ToStringWithoutClients()).ToList());
            _eventBus.Publish(integrationEvent);

            return Result.Ok(client);
        }

        return Result.Fail($"Cannot handle {client.ToStringWithoutId()}");
    }
}

public class GetClientRequestHandler(IClientRepository repository) : IGetClientRequestHandler
{
    private readonly IClientRepository _repository = repository;

    public async Task<Result<Client>> Handle(ClientDto dto, CancellationToken cancellationToken)
    {
        var result = await _repository.GetClientAsync(dto.Id);

        if (result.IsSuccess)
        {
            return Result.Ok(result.Value);
        }

        return Result.Fail(result.Errors);
    }
}

public class GetAllClientsRequestHandler(IClientRepository repository) : IGetAllClientsRequestHandler
{
    private readonly IClientRepository _repository = repository;

    public async Task<Result<IEnumerable<Client>>> Handle() => await _repository.GetAllClientsAsync();
}

public class PutClientRequestHandler(IClientRepository clientRepository, IOrderRepository orderRepository) : IPutClientRequestHandler
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<Client>> Handle(ClientDto dto, CancellationToken cancellationToken)
    {
        var verifyPhoneNumber = false;
        var verifyEmail = false;

        if (!string.IsNullOrEmpty(dto.PhoneNumber))
        {
            verifyPhoneNumber = true;
        }

        if (!string.IsNullOrEmpty(dto.Email))
        {
            verifyEmail = true;
        }

        var client = await Client.CreateClientAsync(dto, _clientRepository, _orderRepository, verifyPhoneNumber, verifyEmail);

        if (client.IsFailed)
        {
            return Result.Fail(client.Errors);
        }

        var result = await _clientRepository.UpdateClientAsync(client.Value);

        if (result is RepositoryResult.Failed)
        {
            return Result.Fail($"Cannot update {client.Value.Id} due to error: {result}");
        }

        if (result is RepositoryResult.NotFound)
        {
            return Result.Fail($"No content");
        }

        return Result.Ok();
    }
}

public class DeleteClientRequestHandler(IClientRepository repository) : IDeleteClientRequestHandler
{
    private readonly IClientRepository _repository = repository;

    public async Task<Result<Client>> Handle(ClientDto dto, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteClientAsync(dto.Id);

        if (result is RepositoryResult.Failed)
        {
            return Result.Fail($"Cannot delete {dto.Name}");
        }

        if (result is RepositoryResult.NotFound)
        {
            return Result.Fail($"No content");
        }

        return Result.Ok();
    }
}

[Mapper]
public partial class ClientMapper
{

}