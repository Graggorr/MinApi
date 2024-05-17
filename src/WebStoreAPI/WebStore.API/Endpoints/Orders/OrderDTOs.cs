using Microsoft.AspNetCore.Mvc;
using WebStore.API.Domain;

namespace WebStore.API.Service.Endpoints.Orders;

//Post
public record PostOrderRequest([FromRoute] Guid ClientId, [FromBody] PostOrderRequestBody RequestBody);
public record PostOrderRequestBody(string Name, string Description, double Price);
public record PostOrderResponse(int Id);

//Put
public record PutOrderRequest([FromRoute] Guid ClientId, [FromBody] PutOrderRequestBody RequestBody);
public record PutOrderRequestBody(int Id, string Name, string Description, double Price);
public record PutOrderResponse(Order Order);

//Delete
public record DeleteOrderRequest(int Id);
public record DeleteOrderResponse(Order Order);

//Get
public record GetOrderRequest(int Id);
public record GetOrderResponse(Order Order);

public record GetClientOrdersRequest(Guid ClientId);
public record GetClientOrdersResponse(List<Order> Orders);