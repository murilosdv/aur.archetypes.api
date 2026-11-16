using Aurore.Archetypes.Api.App.Api.Playground.Shared;

namespace Aurore.Archetypes.Api.App.Api.Playground.Diagnostics;

internal sealed class DiagnosticsResource() : PlaygroundResource("diagnostics", "Diagnostics")
{
    public override bool RequireAuthorization => false;
}
