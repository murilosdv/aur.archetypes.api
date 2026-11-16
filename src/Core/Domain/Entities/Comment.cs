using System;
using Aurore.Foundation.Core.Entities;

namespace Aurore.Archetypes.Api.Core.Domain.Entities;

public sealed class Comment : SoftDeletableEntity
{
    public required string Body { get; set; }

    public Guid PostId { get; init; }

    public Post Post { get; init; } = null!;

    public Guid AuthorId { get; init; }

    public Author Author { get; init; } = null!;
}
