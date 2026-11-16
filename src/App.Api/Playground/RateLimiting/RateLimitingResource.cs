using Aurore.Archetypes.Api.App.Api.Playground.Shared;
namespace Aurore.Archetypes.Api.App.Api.Playground.RateLimiting;

internal sealed class RateLimitingResource() : PlaygroundResource("rate-limiting", "Rate limiting")
{
    public override bool RequireAuthorization => false;
}
