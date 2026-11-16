using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Clients.Email;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Foundation.Core.Extensions;

namespace Aurore.Archetypes.Api.Infra.Clients.Email;

internal sealed class SmtpEmailSender(EmailOptions options) : IEmailSender
{
    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        using var mail = new MailMessage
        {
            From = new MailAddress(options.From),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = false,
        };

        foreach (var address in message.To.Distinct())
            mail.To.Add(address);

        using var client = new SmtpClient(options.Host, options.Port) { EnableSsl = options.EnableSsl };

        if (options.Username.HasValue())
            client.Credentials = new NetworkCredential(options.Username, options.Password);

        await client.SendMailAsync(mail, cancellationToken);
    }
}
