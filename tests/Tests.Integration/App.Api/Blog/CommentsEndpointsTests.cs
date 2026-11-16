using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Tests.Integration.Support;

namespace Aurore.Archetypes.Api.Tests.Integration.App.Api.Blog;

[Collection(BlogCollection.Name)]
[Trait("Feature", "Comments API")]
public sealed class CommentsEndpointsTests(BlogFixture blog)
{
    [Fact(DisplayName = "Someone with no author profile cannot comment: 409 with the code that says to create one")]
    public async Task CommentNeedsAProfile()
    {
        // Arrange
        var (id, _) = await blog.NewPostAsync(publish: true);

        // Act
        using var response = await blog.SendAsync("carol", HttpMethod.Post, $"/v1/posts/{id}/comments", new { body = "Nice." });

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        using var json = await BlogFixture.JsonAsync(response);
        Assert.Equal("author.profile_required", json.RootElement.GetProperty("code").GetString());
    }

    [Fact(DisplayName = "A reader with a profile comments on a published post, and cannot on a draft")]
    public async Task CommentsOnPublishedPostsOnly()
    {
        // Arrange
        var (draft, _) = await blog.NewPostAsync(publish: false);
        var (published, _) = await blog.NewPostAsync(publish: true);

        // Act
        using var onDraft = await blog.SendAsync("alice", HttpMethod.Post, $"/v1/posts/{draft}/comments", new { body = "Early." });
        using var onPublished = await blog.SendAsync("alice", HttpMethod.Post, $"/v1/posts/{published}/comments", new { body = "Nice." });

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, onDraft.StatusCode);
        Assert.Equal(HttpStatusCode.Created, onPublished.StatusCode);
        using var json = await BlogFixture.JsonAsync(onPublished);
        Assert.Equal(blog.HandleOf("alice"), json.RootElement.GetProperty("authorHandle").GetString());
    }

    [Fact(DisplayName = "An empty comment is a 400, and a post that does not exist is a 404")]
    public async Task InvalidAndMissing()
    {
        // Arrange
        var (id, _) = await blog.NewPostAsync(publish: true);

        // Act
        using var empty = await blog.SendAsync("alice", HttpMethod.Post, $"/v1/posts/{id}/comments", new { body = "" });
        using var missing = await blog.SendAsync("alice", HttpMethod.Post, $"/v1/posts/{Guid.NewGuid()}/comments", new { body = "Hello?" });
        using var listMissing = await blog.SendAsync("alice", HttpMethod.Get, $"/v1/posts/{Guid.NewGuid()}/comments");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, empty.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, listMissing.StatusCode);
    }

    [Fact(DisplayName = "Repeating a comment with the same Idempotency-Key leaves one comment")]
    public async Task RepeatedCommentIsIdempotent()
    {
        // Arrange
        var (id, _) = await blog.NewPostAsync(publish: true);
        var key = $"comment-{Guid.NewGuid():N}";

        // Act
        using var first = await blog.SendAsync("alice", HttpMethod.Post, $"/v1/posts/{id}/comments", new { body = "Once." }, key);
        using var second = await blog.SendAsync("alice", HttpMethod.Post, $"/v1/posts/{id}/comments", new { body = "Once." }, key);
        using var list = await blog.SendAsync("alice", HttpMethod.Get, $"/v1/posts/{id}/comments");

        // Assert
        Assert.Equal(HttpStatusCode.Created, second.StatusCode);
        using var json = await BlogFixture.JsonAsync(list);
        Assert.Single(json.RootElement.GetProperty("items").EnumerateArray());
    }

    [Fact(DisplayName = "Following nextCursor lists every comment once, oldest first, as an infinite scroll")]
    public async Task CursorListsEveryComment()
    {
        // Arrange
        var (id, _) = await blog.NewPostAsync(publish: true);
        var written = new List<string>();

        for (var i = 0; i < 5; i++)
        {
            using var created = await blog.SendAsync(i % 2 is 0 ? "alice" : "bob", HttpMethod.Post, $"/v1/posts/{id}/comments", new { body = $"Comment {i}" });
            created.EnsureSuccessStatusCode();
            written.Add($"Comment {i}");
        }

        // Act
        var seen = new List<string>();
        string? cursor = null;
        var pages = 0;

        do
        {
            var path = $"/v1/posts/{id}/comments?pageSize=2" + (cursor is null ? string.Empty : $"&cursor={Uri.EscapeDataString(cursor)}");
            using var response = await blog.SendAsync("carol", HttpMethod.Get, path);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            using var json = await BlogFixture.JsonAsync(response);
            seen.AddRange(json.RootElement.GetProperty("items").EnumerateArray().Select(x => x.GetProperty("body").GetString()!));
            cursor = json.RootElement.TryGetProperty("nextCursor", out var next) && next.ValueKind is System.Text.Json.JsonValueKind.String ? next.GetString() : null;
            pages++;
        }
        while (cursor is not null);

        // Assert
        Assert.Equal(written, seen);
        Assert.Equal(3, pages);
    }

    [Fact(DisplayName = "A reader reports a comment: 202, the moderators get an email with the reason, and the same reader cannot report it twice")]
    public async Task ReportSendsAnEmailAndIsDoneOnce()
    {
        // Arrange
        var (postId, _) = await blog.NewPostAsync(publish: true);
        var text = $"Report me {Guid.NewGuid():N}";
        using var comment = await blog.SendAsync("bob", HttpMethod.Post, $"/v1/posts/{postId}/comments", new { body = text });
        using var commentJson = await BlogFixture.JsonAsync(comment);
        var commentId = commentJson.RootElement.GetProperty("id").GetGuid();
        var path = $"/v1/posts/{postId}/comments/{commentId}/report";

        // Act
        using var reported = await blog.SendAsync("alice", HttpMethod.Post, path, new { reason = "Looks like spam" });
        using var again = await blog.SendAsync("alice", HttpMethod.Post, path, new { reason = "Still spam" });

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, reported.StatusCode);
        using var json = await BlogFixture.JsonAsync(reported);
        Assert.Equal(commentId, json.RootElement.GetProperty("commentId").GetGuid());
        var email = Assert.Single(blog.Emails.Containing(text));
        Assert.Contains("Looks like spam", email.Body);
        Assert.Contains(blog.HandleOf("alice"), email.Body);
        Assert.Contains(blog.HandleOf("bob"), email.Body);
        Assert.Equal(HttpStatusCode.Conflict, again.StatusCode);
        using var conflict = await BlogFixture.JsonAsync(again);
        Assert.Equal("comment.already_reported", conflict.RootElement.GetProperty("code").GetString());
    }

    [Fact(DisplayName = "Reporting needs a sign-in and an author profile, a reason, and a comment that exists")]
    public async Task ReportIsRefusedWhenItShould()
    {
        // Arrange
        var (postId, _) = await blog.NewPostAsync(publish: true);
        using var comment = await blog.SendAsync("bob", HttpMethod.Post, $"/v1/posts/{postId}/comments", new { body = "A comment." });
        using var commentJson = await BlogFixture.JsonAsync(comment);
        var path = $"/v1/posts/{postId}/comments/{commentJson.RootElement.GetProperty("id").GetGuid()}/report";

        // Act
        using var anonymous = await blog.SendAsync(null, HttpMethod.Post, path, new { reason = "Spam" });
        using var noProfile = await blog.SendAsync("carol", HttpMethod.Post, path, new { reason = "Spam" });
        using var noReason = await blog.SendAsync("alice", HttpMethod.Post, path, new { reason = string.Empty });
        using var missing = await blog.SendAsync("alice", HttpMethod.Post, $"/v1/posts/{postId}/comments/{Guid.NewGuid()}/report", new { reason = "Spam" });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, anonymous.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, noProfile.StatusCode);
        using var noProfileJson = await BlogFixture.JsonAsync(noProfile);
        Assert.Equal("author.profile_required", noProfileJson.RootElement.GetProperty("code").GetString());
        Assert.Equal(HttpStatusCode.BadRequest, noReason.StatusCode);
        using var noReasonJson = await BlogFixture.JsonAsync(noReason);
        Assert.True(noReasonJson.RootElement.GetProperty("errors").TryGetProperty("reason", out _));
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }
}
