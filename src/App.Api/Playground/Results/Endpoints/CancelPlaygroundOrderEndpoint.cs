using Aurore.Archetypes.Api.App.Api.Playground.Results.Shared;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Results.Endpoints;

internal sealed class CancelPlaygroundOrderValidator : FeatureValidator<PlaygroundOrder>
{
    public CancelPlaygroundOrderValidator()
    {
        RuleFor(x => x.Status).Equal("pending").WithMessage("Only pending orders can be cancelled.");
    }
}

internal sealed class CancelPlaygroundOrderEndpoint : MinimalEndpoint<ResultsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapPost("orders/{id:int}/cancel", Handle);

        static Results<Ok<PlaygroundOrder>, ProblemHttpResult> Handle(int id)
        {
            return PlaygroundOrders.Cancel(id).ToHttpResult();
        }
    }
}
