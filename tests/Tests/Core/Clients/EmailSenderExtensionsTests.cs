using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Clients.Email;
using Microsoft.Extensions.Logging.Abstractions;

namespace Aurore.Archetypes.Api.Tests.Core.Clients;

[Trait("Feature", "Email sender")]
public sealed class EmailSenderExtensionsTests
{
    private static readonly EmailMessage _message = new(["moderators@example.com"], "Hello", "Text");

    [Fact(DisplayName = "A message that is sent returns true")]
    public async Task SentMessageReturnsTrue()
    {
        // Arrange
        var sender = new StubSender(null);

        // Act
        var sent = await sender.TrySendAsync(_message, NullLogger.Instance, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(sent);
        Assert.Equal(1, sender.Calls);
    }

    [Fact(DisplayName = "A mail server that fails returns false instead of failing the feature that sent the message")]
    public async Task FailureReturnsFalse()
    {
        // Arrange
        var sender = new StubSender(new InvalidOperationException("The mail server is down."));

        // Act
        var sent = await sender.TrySendAsync(_message, NullLogger.Instance, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(sent);
    }

    [Fact(DisplayName = "A caller that gives up still gets the cancellation")]
    public async Task CancellationIsNotSwallowed()
    {
        // Arrange
        var sender = new StubSender(new OperationCanceledException());

        // Act
        var send = () => sender.TrySendAsync(_message, NullLogger.Instance, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(send);
    }

    private sealed class StubSender(Exception? failure) : IEmailSender
    {
        public int Calls { get; private set; }

        public Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
        {
            Calls++;

            return failure is null ? Task.CompletedTask : Task.FromException(failure);
        }
    }
}
