using System;
using Aurore.Foundation.Core.Entities;

namespace Aurore.Archetypes.Api.Core.Domain.Entities;

public sealed class CommentReport : Entity
{
    public required string Reason { get; set; }

    public Guid CommentId { get; init; }

    public Comment Comment { get; init; } = null!;

    public Guid ReporterId { get; init; }

    public Author Reporter { get; init; } = null!;
}
