using System;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Features.Posts;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Tests.Integration.Support;

namespace Aurore.Archetypes.Api.Tests.Integration.Features.Posts;

[Trait("Feature", "Publish a post")]
public sealed class PublishPostEmailTests(FeaturesFixture features) : IClassFixture<FeaturesFixture>
{
    [Fact(DisplayName = "Publishing a post tells the moderators by email, with the title and the author")]
    public async Task PublishingTellsTheModerators()
    {
        // Arrange
        var title = $"Fresh news {Guid.NewGuid():N}";
        var author = await features.NewAuthorAsync();
        var draft = await features.NewPostAsync(author, title: title);

        // Act
        var published = await features.RunAsync<PublishPost, PublishPostRequest, PostResponse>(new PublishPostRequest(draft.Id, author.Caller));

        // Assert
        Assert.True(published.IsSuccess);
        var email = Assert.Single(features.Emails.Containing(title));
        Assert.Equal([FeaturesFixture.ModeratorAddress], email.To);
        Assert.Contains(author.Profile.Handle, email.Body);
        Assert.Contains(draft.Slug, email.Body);
    }

    [Fact(DisplayName = "A post that is already published is a conflict and sends nothing more")]
    public async Task SecondPublicationSendsNothing()
    {
        // Arrange
        var title = $"Once {Guid.NewGuid():N}";
        var author = await features.NewAuthorAsync();
        var post = await features.NewPostAsync(author, publish: true, title: title);

        // Act
        var again = await features.RunAsync<PublishPost, PublishPostRequest, PostResponse>(new PublishPostRequest(post.Id, author.Caller));

        // Assert
        Assert.True(again.IsFailure);
        Assert.Single(features.Emails.Containing(title));
    }

    [Fact(DisplayName = "When the mail server is down the post is still published")]
    public async Task PublishingSurvivesAMailServerThatIsDown()
    {
        // Arrange
        var title = $"Quiet {Guid.NewGuid():N}";
        var author = await features.NewAuthorAsync();
        var draft = await features.NewPostAsync(author, title: title);
        features.Emails.Down = true;

        try
        {
            // Act
            var published = await features.RunAsync<PublishPost, PublishPostRequest, PostResponse>(new PublishPostRequest(draft.Id, author.Caller));

            // Assert
            Assert.True(published.IsSuccess);
            Assert.Empty(features.Emails.Containing(title));
        }
        finally
        {
            features.Emails.Down = false;
        }
    }
}
