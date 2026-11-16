using Aurore.Archetypes.Api.App.Api.Playground.Shared;

namespace Aurore.Archetypes.Api.App.Api.Playground.Idempotency;

internal sealed class IdempotencyResource() : PlaygroundResource("idempotency", "Idempotency")
{
    public override bool RequireAuthorization => false;
}
