using Aurore.Foundation.Core.Constants;

namespace Aurore.Archetypes.Api.Core.Constants;

public sealed class AppPatterns : Patterns
{
    public const string Handle = "^[A-Za-z0-9]+([_-][A-Za-z0-9]+)*$";

    private AppPatterns()
    {
    }
}
