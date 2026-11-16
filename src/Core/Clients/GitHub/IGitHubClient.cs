using System.Threading;
using System.Threading.Tasks;

namespace Aurore.Archetypes.Api.Core.Clients.GitHub;

public interface IGitHubClient
{
    public Task<GitHubUser?> GetUserAsync(string login, CancellationToken cancellationToken);
}
