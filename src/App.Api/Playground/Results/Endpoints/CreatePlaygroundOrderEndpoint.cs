using Aurore.Archetypes.Api.App.Api.Playground.Results.Shared;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Results.Endpoints;

internal sealed class CreatePlaygroundOrderValidator : RequestValidator<CreatePlaygroundOrderRequest>
{
    public CreatePlaygroundOrderValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Quantity).InclusiveBetween(1, 100);
    }
}

internal sealed class CreatePlaygroundOrderEndpoint : MinimalEndpoint<ResultsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapPost("orders", Handle);

        static Results<Created<PlaygroundOrder>, ProblemHttpResult> Handle(CreatePlaygroundOrderRequest request)
        {
            return PlaygroundOrders
                .Create(request)
                .ToHttpResult(order => TypedResults.Created($"/v1/playground/results/orders/{order.Id}", order));
        }
    }
}
