using System;
using Aurore.Archetypes.Api.Core.Domain.Entities;

namespace Aurore.Archetypes.Api.Features.Posts.Shared;

public sealed record PostResponse(
    Guid Id,
    string Slug,
    string Title,
    string Body,
    PostStatus Status,
    string AuthorHandle,
    DateTimeOffset? PublishedAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
