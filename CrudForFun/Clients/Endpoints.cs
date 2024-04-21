using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebStore.Application.Clients;
using WebStore.Domain;
using System.Text;

namespace WebStore.API.Clients;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapClients(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("/client").WithTags("Clients");

        builder.MapPost(string.Empty, PostClient);
        builder.MapGet("/{phoneNumber}", GetClient);
        builder.MapGet(string.Empty, GetAllClients);
        builder.MapPut("/{phoneNumber}", PutClient);
        builder.MapDelete("/{phoneNumber}", DeleteClient);

        return builder;
    }

    private static async Task<Results<Ok<PostClientResponse>, BadRequest<string>>>PostClient(
        [AsParameters] PostClientRequest request,
        [FromServices] IPostClientRequestHandler handler)
    {
        var dto = new ClientDto(request.Name, request.PhoneNumber, request.Email, request.Orders);

        var response = await handler.Handle(dto, CancellationToken.None);

        if (response.IsFailed)
        {
            var stringBuilder = new StringBuilder();
            response.Errors.ForEach(e => stringBuilder.AppendLine(e.Message));

            return TypedResults.BadRequest(stringBuilder.ToString());
        }

        var mappedResponse = new PostClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok<PutClientResponse>, BadRequest<string>, NotFound>> PutClient(
       [AsParameters] PutClientRequest request,
       [FromBody] PutClientRequestBody body,
       [FromServices] IPutClientRequestHandler handler)
    {
        var dto = new ClientDto(body.Name, request.PhoneNumber, body.Email, body.Orders);

        var response = await handler.Handle(dto, CancellationToken.None);

        if (response.IsFailed)
        {
            if(response.Errors.First().Message.Contains("is not contained"))
            {
                return TypedResults.NotFound();
            }

            var stringBuilder = new StringBuilder();
            response.Errors.ForEach(e => stringBuilder.AppendLine(e.Message));

            return TypedResults.BadRequest(stringBuilder.ToString());
        }

        var mappedResponse = new PutClientResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }

    private static async Task<Results<Ok, NotFound>> DeleteClient(
        [AsParameters] DeleteClientRequest request,
        [FromServices] IDeleteClientRequestHandler handler)
    {
        var dto = new ClientDto(string.Empty, request.PhoneNumber, string.Empty, null);
        var response = await handler.Handle(dto, CancellationToken.None);

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<GetClientResponse>, NotFound>> GetClient(
        [AsParameters] GetClientRequest request,
        [FromServices] IGetClientRequestHandler handler)
    {
        var dto = new ClientDto(string.Empty, request.PhoneNumber, string.Empty, null);
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
}