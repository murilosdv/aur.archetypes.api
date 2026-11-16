namespace Aurore.Archetypes.Api.Core.Clients.GitHub;

public sealed record GitHubUser(string Login, long Id, string? Name, string? Company, int PublicRepos, string HtmlUrl);
