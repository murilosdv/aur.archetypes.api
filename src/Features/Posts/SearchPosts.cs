using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Domain;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Pagination.Extensions;
using Aurore.Foundation.Core.Pagination.Models;
using Aurore.Foundation.Core.Results;
using Aurore.Foundation.EntityFrameworkCore.Pagination;

namespace Aurore.Archetypes.Api.Features.Posts;

public sealed class SearchPostsFilter : TimestampPageRequest
{
    public string? Title { get; init; }

    public PostStatus? Status { get; init; }

    public string? Author { get; init; }
}

public sealed record SearchPostsRequest(SearchPostsFilter Filter, Caller Caller);

public sealed class SearchPosts(
    AppDbContext db,
    PaginationOptions pagination)
    : Feature<SearchPostsRequest, Page<PostSummaryResponse>>
{
    private static readonly SortMap<PostSummaryResponse> _sort = SortMap.For<PostSummaryResponse>()
        .Allow(x => x.Title)
        .Allow(x => x.AuthorHandle, "author")
        .Allow(x => x.PublishedAt)
        .DefaultTo(x => x.CreatedAt, SortDirection.Desc);

    protected override async ValueTask<Result<Page<PostSummaryResponse>>> ExecuteAsync(
        SearchPostsRequest request,
        CancellationToken cancellationToken)
    {
        var (filter, caller) = (request.Filter, request.Caller);

        var posts = db.Posts
            .VisibleTo(caller)
            .TitleContaining(filter.Title)
            .WrittenBy(filter.Author);

        if (filter.Status is { } status)
            posts = posts.Where(x => x.Status == status);

        var ranged = posts.WhereTimestamps(filter, x => x.CreatedAt, x => x.UpdatedAt);

        if (ranged.IsFailure)
            return ranged.Error!;

        return await ranged.Value.ToSummaries().ToPageAsync(filter, pagination, _sort, cancellationToken);
    }
}
