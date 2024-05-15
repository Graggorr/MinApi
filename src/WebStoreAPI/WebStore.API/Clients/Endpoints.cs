using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using FluentResults;
using MediatR;
using WebStore.API.Application.Clients.Queries;
using WebStore.API.Application.Clients.Commands;

namespace WebStore.API.Service.Clients;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapClients(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/clients").WithTags("Clients");

        group.MapPost(string.Empty, PostClient);
        group.MapGet("/{page: int}", GetPaginatedClients);
        group.MapGet("/{id: guid}", GetClient).WithName(nameof(GetClient));
        group.MapPut("/{id}", PutClient);
        group.MapDelete("/{id}", DeleteClient);

        return builder;
    }

    private static async Task<Results<CreatedAtRoute<PostClientResponse>, BadRequest<string>>> PostClient(
        [FromBody] PostClientRequest request,
        [FromServices] IMediator mediator)
    {
        var dto = new RegisterClientRequest(Guid.NewGuid(), request.Name, request.PhoneNumber, request.Email);

        var response = await mediator.Send(dto);

        if (response.IsFailed)
        {
            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        var mappedResponse = new PostClientResponse(response.Value);

        return TypedResults.CreatedAtRoute(mappedResponse, nameof(GetClient), new { id = response.Value });
    }

    private static async Task<Results<Ok<PutClientResponse>, BadRequest<string>, NotFound>> PutClient(
       [AsParameters] PutClientRequest request,
       [FromServices] IMediator mediator)
    {
        var dto = new RegisterClientRequest(request.Id, request.Body.Name, request.Body.PhoneNumber, request.Body.Email);
        var handlingRequest = new UpdateClientRequest(dto);

        var response = await mediator.Send(handlingRequest);

        if (response.IsFailed)
        {
            if (response.Errors.First().Message.Contains("is not contained"))
            {
                return TypedResults.NotFound();
            }

            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        var mappedResponse = new PutClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<DeleteClientResponse>, NotFound, BadRequest<string>>> DeleteClient(
        [AsParameters] DeleteClientRequest request,
        [FromServices] IMediator mediator)
    {
        var handlingRequest = new DeleteClientHandlingRequest(request.Id);

        var response = await mediator.Send(handlingRequest);

        if (response.IsFailed)
        {
            if (response.Errors.First().Message.Contains("No content"))
            {
                return TypedResults.NotFound();
            }

            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        var mappedResponse = new DeleteClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<GetClientResponse>, NotFound>> GetClient(
        [AsParameters] GetClientRequest request,
        [FromServices] IMediator mediator)
    {
        var handlingRequest = new GetClientHandlingRequest(request.Id);
        var response = await mediator.Send(handlingRequest);

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        var mappedResponse = new GetClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<GetPaginatedClientsResponse>, NotFound>> GetPaginatedClients(
        [AsParameters] GetPaginatedClientsRequest request,
        [FromServices] IMediator mediator)
    {
        var response = await mediator.Send(new Application.Clients.Queries.GetPaginatedClientsRequest(request.Page));

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        var mappedResponse = new GetPaginatedClientsResponse(response.Value.ToList());

        return TypedResults.Ok(mappedResponse);
    }

    private static string CreateErrorResponse(List<IError> errors)
    {
        var stringBuilder = new StringBuilder();
        errors.ForEach(e => stringBuilder.AppendLine(e.Message));

        return stringBuilder.ToString();
    }
}