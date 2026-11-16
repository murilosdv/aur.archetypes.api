using System;
using System.Linq;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Features.Comments;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Tests.Integration.Support;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Tests.Integration.Features.Comments;

[Trait("Feature", "Report a comment")]
public sealed class ReportCommentTests(FeaturesFixture features) : IClassFixture<FeaturesFixture>
{
    private async Task<(FeaturesFixture.Author Writer, FeaturesFixture.Author Commenter, PostResponse Post, CommentResponse Comment)> NewCommentAsync(string text)
    {
        var writer = await features.NewAuthorAsync("writer");
        var commenter = await features.NewAuthorAsync("commenter");
        var post = await features.NewPostAsync(writer, publish: true, title: $"Post of {Guid.NewGuid():N}");
        var comment = await features.RunAsync<CreateComment, CreateCommentRequest, CommentResponse>(new CreateCommentRequest(commenter.Caller.Subject, post.Id, text));

        return (writer, commenter, post, comment.Value);
    }

    [Fact(DisplayName = "A reported comment is kept with its reason, and the moderators get an email with the post, the comment, its author and the reason")]
    public async Task ReportIsKeptAndModeratorsAreTold()
    {
        // Arrange
        var text = $"Rude words {Guid.NewGuid():N}";
        var (writer, commenter, post, comment) = await NewCommentAsync(text);
        var reporter = await features.NewAuthorAsync("reporter");

        // Act
        var result = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest(reporter.Caller.Subject, post.Id, comment.Id, "Insults the author"));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(comment.Id, result.Value.CommentId);
        var stored = await features.QueryAsync(db => db.CommentReports.SingleAsync(x => x.Id == result.Value.Id, TestContext.Current.CancellationToken));
        Assert.Equal("Insults the author", stored.Reason);
        var email = Assert.Single(features.Emails.Containing(text));
        Assert.Equal([FeaturesFixture.ModeratorAddress], email.To);
        Assert.Contains(post.Title, email.Subject);
        Assert.Contains("Insults the author", email.Body);
        Assert.Contains(post.Slug, email.Body);
        Assert.Contains(reporter.Profile.Handle, email.Body);
        Assert.Contains(commenter.Profile.Handle, email.Body);
        Assert.Contains(writer.Profile.Handle, email.Body);
    }

    [Fact(DisplayName = "An author reports a comment once, and another author can still report it")]
    public async Task ACommentIsReportedOncePerAuthor()
    {
        // Arrange
        var (_, _, post, comment) = await NewCommentAsync("Doubtful.");
        var first = await features.NewAuthorAsync("reporter");
        var second = await features.NewAuthorAsync("reporter");

        // Act
        var firstReport = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest(first.Caller.Subject, post.Id, comment.Id, "Spam"));
        var again = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest(first.Caller.Subject, post.Id, comment.Id, "Still spam"));
        var other = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest(second.Caller.Subject, post.Id, comment.Id, "Spam"));

        // Assert
        Assert.True(firstReport.IsSuccess);
        Assert.Equal(AppErrors.Comments.AlreadyReported, again.Error);
        Assert.True(other.IsSuccess);
    }

    [Fact(DisplayName = "Someone without an author profile cannot report")]
    public async Task ReportNeedsAnAuthorProfile()
    {
        // Arrange
        var (_, _, post, comment) = await NewCommentAsync("Doubtful.");

        // Act
        var result = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest($"subject-{Guid.NewGuid():N}", post.Id, comment.Id, "Spam"));

        // Assert
        Assert.Equal(AppErrors.Authors.ProfileRequired.Code, result.Error!.Code);
    }

    [Fact(DisplayName = "A comment that does not exist, or that belongs to another post, or was deleted, is a not found, and nothing is sent")]
    public async Task MissingCommentIsNotFound()
    {
        // Arrange
        var (_, _, post, comment) = await NewCommentAsync($"Gone {Guid.NewGuid():N}");
        var (_, _, otherPost, _) = await NewCommentAsync("Elsewhere.");
        var reporter = await features.NewAuthorAsync("reporter");
        await features.RunAsync<DeleteComment, DeleteCommentRequest>(new DeleteCommentRequest(post.Id, comment.Id));

        // Act
        var nothing = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest(reporter.Caller.Subject, post.Id, Guid.NewGuid(), "Spam"));
        var wrongPost = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest(reporter.Caller.Subject, otherPost.Id, comment.Id, "Spam"));
        var deleted = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest(reporter.Caller.Subject, post.Id, comment.Id, "Spam"));

        // Assert
        Assert.Equal(AppErrors.Comments.NotFound, nothing.Error);
        Assert.Equal(AppErrors.Comments.NotFound, wrongPost.Error);
        Assert.Equal(AppErrors.Comments.NotFound, deleted.Error);
        Assert.Empty(features.Emails.Containing(comment.Body));
    }

    [Fact(DisplayName = "When the mail server is down the report is still kept and the author still gets an answer")]
    public async Task ReportSurvivesAMailServerThatIsDown()
    {
        // Arrange
        var (_, _, post, comment) = await NewCommentAsync($"Offensive {Guid.NewGuid():N}");
        var reporter = await features.NewAuthorAsync("reporter");
        features.Emails.Down = true;

        try
        {
            // Act
            var result = await features.RunAsync<ReportComment, ReportCommentRequest, CommentReportResponse>(new ReportCommentRequest(reporter.Caller.Subject, post.Id, comment.Id, "Offensive"));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, await features.QueryAsync(db => db.CommentReports.CountAsync(x => x.CommentId == comment.Id, TestContext.Current.CancellationToken)));
            Assert.Empty(features.Emails.Containing(comment.Body));
        }
        finally
        {
            features.Emails.Down = false;
        }
    }
}
