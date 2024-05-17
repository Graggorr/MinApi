using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using FluentResults;
using MediatR;
using WebStore.API.Application.Orders.Commands;

namespace WebStore.API.Service.Endpoints.Orders;

public static partial class Endpoints
{
    public static IEndpointRouteBuilder MapOrders(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/orders").WithTags("Orders");

        group.MapPost("/{clientId:guid}", PostOrder).WithName(nameof(PostOrder));
        group.MapGet("/{clientId:guid}", GetClientOrders).WithName(nameof(GetClientOrders));
        group.MapGet("/{id:int}", GetOrder).WithName(nameof(GetOrder));
        group.MapPut("/{clientId:guid}", PutOrder).WithName(nameof(PutOrder));
        group.MapDelete("/{id:int}", DeleteOrder).WithName(nameof(DeleteOrder));

        return builder;
    }

    private static async Task<Results<CreatedAtRoute<PostOrderResponse>, BadRequest<string>>> PostOrder(
        [FromBody] PostOrderRequest request,
        [FromServices] IMediator mediator)
    {
        var requestBody = request.RequestBody;
        var registerOrderRequest = new RegisterOrderRequest(request.ClientId, requestBody.Name, requestBody.Price, requestBody.Description);

        var response = await mediator.Send(registerOrderRequest);

        if (response.IsFailed)
        {
            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        var mappedResponse = new PostOrderResponse(response.Value);

        return TypedResults.CreatedAtRoute(mappedResponse, nameof(GetOrder), new { id = response.Value });
    }

    private static async Task<Results<Ok<PutOrderResponse>, BadRequest<string>, NotFound>> PutOrder(
       [AsParameters] PutOrderRequest request,
       [FromServices] IMediator mediator)
    {
        var requestBody = request.RequestBody;
        var handlingRequest = new UpdateOrderRequest(request.ClientId, requestBody.Id, requestBody.Name, requestBody.Price, requestBody.Description);

        var response = await mediator.Send(handlingRequest);

        if (response.IsFailed)
        {
            if (response.Errors.First().Message.Contains("is not contained"))
            {
                return TypedResults.NotFound();
            }

            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        var mappedResponse = new PutOrderResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<DeleteOrderResponse>, NotFound, BadRequest<string>>> DeleteOrder(
        [AsParameters] DeleteOrderRequest request,
        [FromServices] IMediator mediator)
    {
        var handlingRequest = new Application.Orders.Commands.DeleteOrderRequest(request.Id);

        var response = await mediator.Send(handlingRequest);

        if (response.IsFailed)
        {
            if (response.Errors.First().Message.Contains("No content"))
            {
                return TypedResults.NotFound();
            }

            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        var mappedResponse = new DeleteOrderResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<GetOrderResponse>, NotFound>> GetOrder(
        [AsParameters] GetOrderRequest request,
        [FromServices] IMediator mediator)
    {
        var handlingRequest = new Application.Orders.Queries.GetOrderRequest(request.Id);
        var response = await mediator.Send(handlingRequest);

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        var mappedResponse = new GetOrderResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<GetClientOrdersResponse>, NotFound>> GetClientOrders(
        [AsParameters] GetClientOrdersRequest request,
        [FromServices] IMediator mediator)
    {
        var response = await mediator.Send(new Application.Orders.Queries.GetClientOrdersRequest(request.ClientId));

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        var mappedResponse = new GetClientOrdersResponse(response.Value.ToList());

        return TypedResults.Ok(mappedResponse);
    }

    private static string CreateErrorResponse(List<IError> errors)
    {
        var stringBuilder = new StringBuilder();
        errors.ForEach(e => stringBuilder.AppendLine(e.Message));

        return stringBuilder.ToString();
    }
}