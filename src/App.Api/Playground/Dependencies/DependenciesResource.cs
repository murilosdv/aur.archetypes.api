using Aurore.Archetypes.Api.App.Api.Playground.Shared;
namespace Aurore.Archetypes.Api.App.Api.Playground.Dependencies;

internal sealed class DependenciesResource() : PlaygroundResource("dependencies", "Upstream dependencies")
{
    public override bool RequireAuthorization => false;
}
