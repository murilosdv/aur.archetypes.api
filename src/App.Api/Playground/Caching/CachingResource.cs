using Aurore.Archetypes.Api.App.Api.Playground.Shared;

namespace Aurore.Archetypes.Api.App.Api.Playground.Caching;

internal sealed class CachingResource() : PlaygroundResource("caching", "Output caching")
{
    public override bool RequireAuthorization => false;
}
