using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebStore.Application.Clients;
using WebStore.Domain;
using System.Text;
using FluentResults;
using MediatR;

namespace WebStore.API.Clients;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapClients(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/client").WithTags("Clients");

        group.MapPost(string.Empty, PostClient);
        group.MapGet(string.Empty, GetAllClients);
        group.MapGet("/{id}", GetClient);
        group.MapPut("/{id}", PutClient);
        group.MapDelete("/{id}", DeleteClient);

        return builder;
    }

    private static async Task<Results<Ok<PostClientResponse>, BadRequest<string>>> PostClient(
        [FromBody] PostClientRequestBody requestBody,
        [FromServices] IRequestHandler<PostClientHandlingRequest, Result<Client>> handler)
    {
        var dto = new ClientDto(Guid.Empty, requestBody.Name, requestBody.PhoneNumber, requestBody.Email, requestBody.Orders);
        var request = new PostClientHandlingRequest(dto);

        var response = await handler.Handle(request, CancellationToken.None);

        if (response.IsFailed)
        {
            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        var mappedResponse = new PostClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<PutClientResponse>, BadRequest<string>, NotFound>> PutClient(
       [AsParameters] PutClientRequest request,
       [FromBody] PutClientRequestBody requestBody,
       [FromServices] IRequestHandler<PutClientHandlingRequest, Result<Client>> handler)
    {
        var dto = new ClientDto(request.Id, requestBody.Name, requestBody.PhoneNumber, requestBody.Email, requestBody.Orders);
        var handlingRequest = new PutClientHandlingRequest(dto);

        var response = await handler.Handle(handlingRequest, CancellationToken.None);

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

    private static async Task<Results<Ok, NotFound, BadRequest<string>>> DeleteClient(
        [AsParameters] DeleteClientRequest request,
        [FromServices] IRequestHandler<DeleteClientHandlingRequest, Result> handler)
    {
        var handlingRequest = new DeleteClientHandlingRequest(request.Id);

        var response = await handler.Handle(handlingRequest, CancellationToken.None);

        if (response.IsFailed)
        {
            if (response.Errors.First().Message.Contains("No content"))
            {
                return TypedResults.NotFound();
            }

            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<GetClientResponse>, NotFound>> GetClient(
        [AsParameters] GetClientRequest request,
        [FromServices] IRequestHandler<GetClientHandlingRequest, Result<Client>> handler)
    {
        var handlingRequest = new GetClientHandlingRequest(request.Id);
        var response = await handler.Handle(handlingRequest, CancellationToken.None);

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        var mappedResponse = new GetClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<GetAllClientsResponse>, NotFound>> GetAllClients(
        [FromServices] IRequestHandler<GetAllClientsHandlingRequest, Result<IEnumerable<Client>>> handler)
    {
        var response = await handler.Handle(new GetAllClientsHandlingRequest(), CancellationToken.None);

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        var mappedResponse = new GetAllClientsResponse(response.Value.ToList());

        return TypedResults.Ok(mappedResponse);
    }

    private static string CreateErrorResponse(List<IError> errors)
    {
        var stringBuilder = new StringBuilder();
        errors.ForEach(e => stringBuilder.AppendLine(e.Message));

        return stringBuilder.ToString();
    }
}