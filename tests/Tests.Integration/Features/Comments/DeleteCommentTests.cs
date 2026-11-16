using System;
using System.Linq;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Features.Comments;
using Aurore.Archetypes.Api.Tests.Integration.Support;
using Aurore.Foundation.Core.Pagination.Models;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Tests.Integration.Features.Comments;

[Trait("Feature", "Comment deletion")]
public sealed class DeleteCommentTests(FeaturesFixture features) : IClassFixture<FeaturesFixture>
{
    [Fact(DisplayName = "A deleted comment leaves the list, and its row is kept marked as deleted")]
    public async Task DeletedCommentLeavesTheList()
    {
        // Arrange
        var writer = await features.NewAuthorAsync();
        var post = await features.NewPostAsync(writer, publish: true);
        var kept = (await features.RunAsync<CreateComment, CreateCommentRequest, CommentResponse>(new CreateCommentRequest(writer.Caller.Subject, post.Id, "Keep"))).Value;
        var removed = (await features.RunAsync<CreateComment, CreateCommentRequest, CommentResponse>(new CreateCommentRequest(writer.Caller.Subject, post.Id, "Remove"))).Value;

        // Act
        var result = await features.RunAsync<DeleteComment, DeleteCommentRequest>(new DeleteCommentRequest(post.Id, removed.Id));
        var list = await features.RunAsync<ListComments, ListCommentsRequest, CursorPage<CommentSummaryResponse>>(new ListCommentsRequest { PostId = post.Id });
        var row = await features.QueryAsync(db => db.Comments.IgnoreQueryFilters().SingleAsync(x => x.Id == removed.Id));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal([kept.Id], list.Value.Items.Select(x => x.Id));
        Assert.NotNull(row.DeletedAt);
    }

    [Fact(DisplayName = "A comment that does not exist, was deleted, or belongs to another post is a not found")]
    public async Task MissingDeletedAndMisplacedCommentsAreNotFound()
    {
        // Arrange
        var writer = await features.NewAuthorAsync();
        var post = await features.NewPostAsync(writer, publish: true);
        var other = await features.NewPostAsync(writer, publish: true);
        var comment = (await features.RunAsync<CreateComment, CreateCommentRequest, CommentResponse>(new CreateCommentRequest(writer.Caller.Subject, post.Id, "Hello"))).Value;

        // Act
        var wrongPost = await features.RunAsync<DeleteComment, DeleteCommentRequest>(new DeleteCommentRequest(other.Id, comment.Id));
        var missing = await features.RunAsync<DeleteComment, DeleteCommentRequest>(new DeleteCommentRequest(post.Id, Guid.NewGuid()));
        await features.RunAsync<DeleteComment, DeleteCommentRequest>(new DeleteCommentRequest(post.Id, comment.Id));
        var again = await features.RunAsync<DeleteComment, DeleteCommentRequest>(new DeleteCommentRequest(post.Id, comment.Id));

        // Assert
        Assert.Equal(AppErrors.Comments.NotFound, wrongPost.Error);
        Assert.Equal(AppErrors.Comments.NotFound, missing.Error);
        Assert.Equal(AppErrors.Comments.NotFound, again.Error);
    }
}
