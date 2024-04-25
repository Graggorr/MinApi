using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebStore.Application.Clients;
using WebStore.Domain;
using System.Text;
using FluentResults;

namespace WebStore.API.Clients;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapClients(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/client").WithTags("Clients");

        group.MapPost(string.Empty, PostClient);
        group.MapGet("/{id}", GetClient);
        group.MapGet(string.Empty, GetAllClients);
        group.MapPut("/{id}", PutClient);
        group.MapDelete("/{id}", DeleteClient);

        return builder;
    }

    private static async Task<Results<Ok<PostClientResponse>, BadRequest<string>>> PostClient(
        [AsParameters] PostClientRequest request,
        [FromServices] IPostClientRequestHandler handler)
    {
        var dto = new ClientDto(Guid.Empty, request.Name, request.PhoneNumber, request.Email, request.Orders);

        var response = await handler.Handle(dto, CancellationToken.None);

        if (response.IsFailed)
        {
            return TypedResults.BadRequest(CreateErrorResponse(response.Errors));
        }

        var mappedResponse = new PostClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<PutClientResponse>, BadRequest<string>, NotFound>> PutClient(
       [AsParameters] PutClientRequest request,
       [FromBody] PutClientRequestBody body,
       [FromServices] IPutClientRequestHandler handler)
    {
        var dto = new ClientDto(request.Id, body.Name, body.PhoneNumber, body.Email, body.Orders);

        var response = await handler.Handle(dto, CancellationToken.None);

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
        [FromServices] IDeleteClientRequestHandler handler)
    {
        var dto = new ClientDto(request.Id, string.Empty, string.Empty, string.Empty, null);
        var response = await handler.Handle(dto, CancellationToken.None);

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
        [FromServices] IGetClientRequestHandler handler)
    {
        var dto = new ClientDto(request.Id, string.Empty, string.Empty, string.Empty, null);
        var response = await handler.Handle(dto, CancellationToken.None);

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        var mappedResponse = new GetClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<GetAllClientsResponse>, NotFound>> GetAllClients(
        [AsParameters] GetClientRequest request,
        [FromServices] IGetAllClientsRequestHandler handler)
    {
        var response = await handler.Handle();

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