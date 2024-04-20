using Application.Phones;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;

namespace WebStore.API.Phones;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapPhones(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("/phone")
            .WithTags("Phones");

        builder.MapGet("/{Id}", GetPhone);
        // builder.MapPost("/{Id}", GetPhone);

        return builder;
    }

    private static async Task<Results<Ok<GetPhoneResponse>, NotFound>> GetPhone(
        [AsParameters] GetPhoneRequest request,
        [FromServices] IPhonesService service)
    {
        var response = await service.GetPhoneAsync(request.Id);

        if (response.IsFailed)
        {
            TypedResults.NotFound();
        }

        var mappedResponse = new GetPhoneResponse(response.Value);

        return TypedResults.Ok(mappedResponse);
    }
}