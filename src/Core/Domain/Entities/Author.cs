using Aurore.Foundation.Core.Entities;

namespace Aurore.Archetypes.Api.Core.Domain.Entities;

public sealed class Author : Entity
{
    public required string Subject { get; init; }

    public required string Handle { get; init; }

    public required string DisplayName { get; set; }
}
