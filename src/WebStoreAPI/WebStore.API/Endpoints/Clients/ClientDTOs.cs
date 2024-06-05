using Microsoft.AspNetCore.Mvc;
using WebStore.API.Application.Clients;

namespace WebStore.API.Service.Endpoints.Clients;

//Post
public record PostClientRequest(string PhoneNumber, string Name, string Email);
public record PostClientResponse(Guid Id);

//Put
public record PutClientRequest([FromRoute] Guid Id, [FromBody] PutClientRequestBody Body);
public record PutClientRequestBody(string Name, string PhoneNumber, string Email);
public record PutClientResponse(ClientData Client);

//Delete
public record DeleteClientRequest(Guid Id);
public record DeleteClientResponse(ClientData Client);

//Get
public record GetClientRequest(Guid Id);
public record GetClientResponse(ClientData Client);

public record GetPaginatedClientsRequest(int Page);
public record GetPaginatedClientsResponse(IEnumerable<ClientData> Clients);