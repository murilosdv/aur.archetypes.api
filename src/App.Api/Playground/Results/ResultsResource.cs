using Aurore.Archetypes.Api.App.Api.Playground.Shared;

namespace Aurore.Archetypes.Api.App.Api.Playground.Results;

internal sealed class ResultsResource() : PlaygroundResource("results", "Results and errors")
{
    public override bool RequireAuthorization => false;
}
