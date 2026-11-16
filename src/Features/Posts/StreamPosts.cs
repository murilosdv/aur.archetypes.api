using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Pagination.Extensions;
using Aurore.Foundation.Core.Pagination.Models;
using Aurore.Foundation.Core.Results;
using Aurore.Foundation.EntityFrameworkCore.Pagination;

namespace Aurore.Archetypes.Api.Features.Posts;

public sealed class StreamPostsRequest : TimestampCursorRequest
{
    public string? Title { get; init; }

    public string? Author { get; init; }
}

public sealed class StreamPosts(
    AppDbContext db,
    PaginationOptions pagination)
    : Feature<StreamPostsRequest, CursorPage<PostSummaryResponse>>
{
    private static readonly SortMap<PostSummaryResponse> _sort = SortMap.For<PostSummaryResponse>()
        .Allow(x => x.Title)
        .Allow(x => x.AuthorHandle, "author")
        .DefaultTo(x => x.CreatedAt, SortDirection.Desc);

    protected override async ValueTask<Result<CursorPage<PostSummaryResponse>>> ExecuteAsync(
        StreamPostsRequest request,
        CancellationToken cancellationToken)
    {
        var posts = db.Posts
            .Where(x => x.Status == PostStatus.Published)
            .TitleContaining(request.Title)
            .WrittenBy(request.Author);

        var ranged = posts.WhereTimestamps(request, x => x.CreatedAt, x => x.UpdatedAt);

        if (ranged.IsFailure)
            return ranged.Error!;

        return await ranged.Value.ToSummaries().ToCursorPageAsync(request, pagination, _sort, cancellationToken);
    }
}
