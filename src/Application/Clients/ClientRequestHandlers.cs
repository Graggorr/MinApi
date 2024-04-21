using FluentResults;
using Riok.Mapperly.Abstractions;
using WebStore.Domain;
using MediatR;
using MapsterMapper;

namespace WebStore.Application.Clients;

public class PostClientRequestHandler(IClientRepository clientRepository, IOrderRepository orderRepository, ClientMapper mapper) : IPostClientRequestHandler
{
    //CQRS
    private readonly IClientRepository _repository = clientRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ClientMapper _mapper = mapper;

    public async Task<Result<Client>> Handle(ClientDto dto, CancellationToken cancellationToken)
    {
        var client = await Client.CreateClientAsync(dto, _repository, _orderRepository);

        if (client.IsFailed)
        {
            return Result.Fail(client.Errors);
        }

        var result = await _repository.PostClientAsync(client.Value);

        if (result.IsSuccess)
        {
            return Result.Ok(client.Value);
        }

        return Result.Fail(result.Errors);
    }
}

public class GetClientRequestHandler(IClientRepository repository) : IGetClientRequestHandler
{
    private readonly IClientRepository _repository = repository;

    public async Task<Result<Client>> Handle(ClientDto dto, CancellationToken cancellationToken)
    {
        var result = await _repository.GetClientAsync(dto.PhoneNumber);

        if (result.IsSuccess)
        {
            return Result.Ok(result.Value);
        }

        return Result.Fail(result.Errors);
    }
}

public class GetAllClientsRequestHandler(IClientRepository repository): IGetAllClientsRequestHandler
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
        var client = await Client.CreateClientAsync(dto, _clientRepository, _orderRepository, false, true);

        if (client.IsFailed)
        {
            return Result.Fail(client.Errors);
        }

        var result = await _clientRepository.PutClientAsync(client.Value);

        if (result.IsSuccess)
        {
            return Result.Ok();
        }

        return Result.Fail(result.Errors);
    }
}

public class DeleteClientRequestHandler(IClientRepository repository) : IDeleteClientRequestHandler
{
    private readonly IClientRepository _repository = repository;

    public async Task<Result<Client>> Handle(ClientDto dto, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteClientAsync(dto.PhoneNumber);

        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        return Result.Ok();
    }
}

[Mapper]
public partial class ClientMapper
{

}