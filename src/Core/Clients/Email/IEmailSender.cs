using System.Threading;
using System.Threading.Tasks;

namespace Aurore.Archetypes.Api.Core.Clients.Email;

public interface IEmailSender
{
    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken);
}
