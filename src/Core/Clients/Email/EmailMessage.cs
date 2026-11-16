using System.Collections.Generic;

namespace Aurore.Archetypes.Api.Core.Clients.Email;

public sealed record EmailMessage(IReadOnlyList<string> To, string Subject, string Body)
{
    public string Subject { get; } = Subject.ReplaceLineEndings(" ");
}
