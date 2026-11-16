using Aurore.Archetypes.Api.App.Api.Playground.Shared;
namespace Aurore.Archetypes.Api.App.Api.Playground.Timeouts;

internal sealed class TimeoutsResource() : PlaygroundResource("timeouts", "Request timeouts")
{
    public override bool RequireAuthorization => false;
}
