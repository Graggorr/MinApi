using WebStore.API.Orders;
using WebStore.Domain;

namespace WebStore.API.Clients;

//Post
public record PostClientRequest(string PhoneNumber, string Name, string Email, IList<OrderDto> Orders);
public record PostClientResponse(Client Client); 

//Put
public record PutClientRequest(string PhoneNumber);
public record PutClientRequestBody(string Name, string Email, IList<OrderDto> Orders);
public record PutClientsPhoneNumberRequest(string OldPhoneNumber);
public record PutClientsPhoneNumberRequestBody(string NewPhoneNumber);
public record PutClientResponse(Client Client);

//Delete
public record DeleteClientRequest(string PhoneNumber);

//Get
public record GetClientRequest(string PhoneNumber);
public record GetAllClientsRequest();
public record GetAllClientsResponse(IList<Client> Clients);
public record GetClientResponse(Client Client);