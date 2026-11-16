using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Clients.Email;

namespace Aurore.Archetypes.Api.Tests.Integration.Support;

public sealed class RecordingEmailSender : IEmailSender
{
    private readonly ConcurrentQueue<EmailMessage> _sent = new();

    public bool Down { get; set; }

    public IReadOnlyList<EmailMessage> Sent => [.. _sent];

    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        if (Down)
            throw new InvalidOperationException("The mail server is down.");

        _sent.Enqueue(message);

        return Task.CompletedTask;
    }

    public IReadOnlyList<EmailMessage> Containing(string text)
    {
        return [.. _sent.Where(x => x.Subject.Contains(text, StringComparison.Ordinal) || x.Body.Contains(text, StringComparison.Ordinal))];
    }
}
