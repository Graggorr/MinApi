using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;

namespace WebStore.API.Clients;

//Post
public record PostClientRequest(string PhoneNumber, string Name, string Email, List<OrderSwaggerDto> Orders);
public record PostClientResponse(Guid Id);

//Put
public record PutClientRequest([FromRoute] Guid Id, [FromBody] PutClientRequestBody Body);
public record PutClientRequestBody(string Name, string PhoneNumber, string Email, List<OrderSwaggerDto> Orders);
public record PutClientResponse(Client Client);

//Delete
public record DeleteClientRequest(Guid Id);
public record DeleteClientResponse(Client Client);

//Get
public record GetClientRequest(Guid Id);
public record GetClientResponse(Client Client);

public record GetAllClientsResponse(List<Client> Clients);