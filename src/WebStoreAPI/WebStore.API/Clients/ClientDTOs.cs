using Microsoft.AspNetCore.Mvc;
using WebStore.API.Domain;

namespace WebStore.API.Service.Clients;

//Post
public record PostClientRequest(string PhoneNumber, string Name, string Email);
public record PostClientResponse(Guid Id);

//Put
public record PutClientRequest([FromRoute] Guid Id, [FromBody] PutClientRequestBody Body);
public record PutClientRequestBody(string Name, string PhoneNumber, string Email);
public record PutClientResponse(Client Client);

//Delete
public record DeleteClientRequest(Guid Id);
public record DeleteClientResponse(Client Client);

//Get
public record GetClientRequest(Guid Id);
public record GetClientResponse(Client Client);

public record GetPaginatedClientsRequest(int Page);
public record GetPaginatedClientsResponse(IEnumerable<Client> Clients);