﻿using WebStore.Domain;

namespace WebStore.API.Clients;

//Post
public record PostClientRequestBody(string PhoneNumber, string Name, string Email, IList<OrderDto> Orders);
public record PostClientResponse(Client Client); 

//Put
public record PutClientRequest(Guid Id);
public record PutClientRequestBody(string Name, string PhoneNumber, string Email, IList<OrderDto> Orders);
public record PutClientResponse(Client Client);

//Delete
public record DeleteClientRequest(Guid Id);

//Get
public record GetClientRequest(Guid Id);
public record GetClientResponse(Client Client);

public record GetAllClientsResponse(IList<Client> Clients);