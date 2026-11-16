using System;
using Aurore.Archetypes.Api.Core.Domain.Entities;

namespace Aurore.Archetypes.Api.Features.Posts.Shared;

public sealed record PostSummaryResponse
{
    public required Guid Id { get; init; }

    public required string Slug { get; init; }

    public required string Title { get; init; }

    public required PostStatus Status { get; init; }

    public required string AuthorHandle { get; init; }

    public DateTimeOffset? PublishedAt { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }
}
