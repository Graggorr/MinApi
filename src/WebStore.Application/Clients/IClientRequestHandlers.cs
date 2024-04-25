using FluentResults;
using MediatR;
using WebStore.Domain;

namespace WebStore.Application.Clients;

public interface IPostClientRequestHandler: IRequestHandler<ClientDto, Result<Client>>
{

}

public interface IGetClientRequestHandler : IRequestHandler<ClientDto, Result<Client>>
{

}
public interface IGetAllClientsRequestHandler
{
    public Task<Result<IEnumerable<Client>>> Handle();
}
public interface IPutClientRequestHandler : IRequestHandler<ClientDto, Result<Client>>
{

}
public interface IDeleteClientRequestHandler : IRequestHandler<ClientDto, Result<Client>>
{

}

public interface IClientRequestHandlers
{
    public Task<Result<Client>> CreateClientAsync(string phoneNumber, string email, string name, IList<Order> orders);
    public Task<Result<Client>> GetClientAsync(string phoneNumber);
    public Task<Result<Client>> RemoveClientAsync(string phoneNumber);
    public Task<Result<Client>> UpdateClientAsync(string phoneNumber, string email, string name, IList<Order> orders);
    public Task<Result<Client>> UpdateClientsPhoneNumberAsync(string oldPhoneNumber, string newPhoneNumber);
}