using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Aurore.Archetypes.Api.Core.Clients.Email;

public static partial class EmailSenderExtensions
{
    public static async Task<bool> TrySendAsync(
        this IEmailSender sender,
        EmailMessage message,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        try
        {
            await sender.SendAsync(message, cancellationToken);

            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            LogNotSent(logger, ex, message.Subject, message.To.Count);

            return false;
        }
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "The email \"{Subject}\" to {Recipients} recipient(s) could not be sent")]
    private static partial void LogNotSent(ILogger logger, Exception exception, string subject, int recipients);
}
