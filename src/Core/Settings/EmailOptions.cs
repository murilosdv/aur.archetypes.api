using System.Collections.Generic;

namespace Aurore.Archetypes.Api.Core.Settings;

public sealed record EmailOptions
{
    public required string Host { get; init; }

    public int Port { get; init; } = 587;

    public bool EnableSsl { get; init; } = true;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public required string From { get; init; }

    public IReadOnlyList<string> ModeratorAddresses { get; init; } = [];
}
