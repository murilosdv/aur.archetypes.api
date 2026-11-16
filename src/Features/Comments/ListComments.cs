using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Pagination.Extensions;
using Aurore.Foundation.Core.Pagination.Models;
using Aurore.Foundation.Core.Results;
using Aurore.Foundation.EntityFrameworkCore.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Comments;

public sealed record CommentSummaryResponse
{
    public required Guid Id { get; init; }

    public required string Body { get; init; }

    public required string AuthorHandle { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }
}

public sealed class ListCommentsRequest : TimestampCursorRequest
{
    public Guid PostId { get; init; }
}

public sealed class ListComments(
    AppDbContext db,
    PaginationOptions pagination)
    : Feature<ListCommentsRequest, CursorPage<CommentSummaryResponse>>
{
    private static readonly SortMap<CommentSummaryResponse> _sort = SortMap.For<CommentSummaryResponse>()
        .DefaultTo(x => x.CreatedAt, SortDirection.Asc);

    protected override async ValueTask<Result<CursorPage<CommentSummaryResponse>>> ExecuteAsync(
        ListCommentsRequest request,
        CancellationToken cancellationToken)
    {
        if (await db.Posts.AnyAsync(x => x.Id == request.PostId, cancellationToken) is false)
            return AppErrors.Posts.NotFound;

        var ranged = db.Comments
            .Where(x => x.PostId == request.PostId)
            .WhereTimestamps(request, x => x.CreatedAt, x => x.UpdatedAt);

        if (ranged.IsFailure)
            return ranged.Error!;

        return await ranged.Value
            .Select(x => new CommentSummaryResponse { Id = x.Id, Body = x.Body, AuthorHandle = x.Author.Handle, CreatedAt = x.CreatedAt })
            .ToCursorPageAsync(request, pagination, _sort, cancellationToken);
    }
}
