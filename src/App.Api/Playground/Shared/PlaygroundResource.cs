using Aurore.Foundation.AspNetCore.MinimalApis;

namespace Aurore.Archetypes.Api.App.Api.Playground.Shared;

internal abstract class PlaygroundResource(string topic, string title) : ResourceGroup
{
    public override string Name => $"playground/{topic}";

    public override string[] Tags => [$"Playground: {title}"];
}
