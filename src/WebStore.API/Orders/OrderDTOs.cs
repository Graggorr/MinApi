using WebStore.Domain;

namespace WebStore.API.Orders;

//Post
public record PostOrderRequest(string Name, string Description, double Price);
public record PostOrderResponse(Order Order);

//Put
public record UpdateOrderRequest(int Id);
public record UpdateOrderRequestBody(string Name, string Description, double Price);
public record UpdateOrderResponse(Order Order);

//Delete
public record DeleteOrderRequest(int Id);

//Get
public record GetOrderRequest(int Id);
public record GetAllOrdersRequest();
public record GetAllOrdersResponse(IList<Order> Orders);
public record GetOrderResponse(Order Order);