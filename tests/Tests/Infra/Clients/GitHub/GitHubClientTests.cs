using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Infra.Clients.GitHub;
using Aurore.Archetypes.Api.Tests.Support;

namespace Aurore.Archetypes.Api.Tests.Infra.Clients.GitHub;

[Trait("Feature", "GitHub client")]
public sealed class GitHubClientTests
{
    private const string UserJson = """
        { "login": "octocat", "id": 583231, "name": "The Octocat", "company": "@github", "public_repos": 8, "html_url": "https://github.com/octocat" }
        """;

    [Fact(DisplayName = "A user is read from the snake case document GitHub answers with")]
    public async Task UserIsReadFromTheGitHubDocument()
    {
        // Arrange
        var handler = new StubHttpHandler(HttpStatusCode.OK, UserJson);
        var client = CreateClient(handler);

        // Act
        var user = await client.GetUserAsync("octocat", TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(user);
        Assert.Equal("octocat", user.Login);
        Assert.Equal(583231, user.Id);
        Assert.Equal("The Octocat", user.Name);
        Assert.Equal(8, user.PublicRepos);
        Assert.Equal("https://github.com/octocat", user.HtmlUrl);
    }

    [Fact(DisplayName = "The login is escaped in the address, so it cannot reach another path")]
    public async Task LoginIsEscapedInTheAddress()
    {
        // Arrange
        var handler = new StubHttpHandler(HttpStatusCode.OK, UserJson);
        var client = CreateClient(handler);

        // Act
        await client.GetUserAsync("../orgs/secret", TestContext.Current.CancellationToken);

        // Assert
        Assert.Contains("..%2Forgs%2Fsecret", handler.Request!.RequestUri!.OriginalString, StringComparison.OrdinalIgnoreCase);
    }

    [Fact(DisplayName = "A user that does not exist is null, not an error")]
    public async Task MissingUserIsNull()
    {
        // Arrange
        var client = CreateClient(new StubHttpHandler(HttpStatusCode.NotFound));

        // Act
        var user = await client.GetUserAsync("nobody", TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(user);
    }

    [Fact(DisplayName = "Any other failure of GitHub is an error for the caller to handle")]
    public async Task OtherFailuresAreErrors()
    {
        // Arrange
        var client = CreateClient(new StubHttpHandler(HttpStatusCode.InternalServerError));

        // Act
        var act = () => client.GetUserAsync("octocat", TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<HttpRequestException>(act);
    }

    private static GitHubClient CreateClient(StubHttpHandler handler)
    {
        return new GitHubClient(new HttpClient(handler) { BaseAddress = new Uri("https://api.github.test/") });
    }
}
