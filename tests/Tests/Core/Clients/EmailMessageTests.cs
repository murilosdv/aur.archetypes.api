using Aurore.Archetypes.Api.Core.Clients.Email;

namespace Aurore.Archetypes.Api.Tests.Core.Clients;

[Trait("Feature", "Email message")]
public sealed class EmailMessageTests
{
    [Theory(DisplayName = "The subject is one line: line breaks become spaces, so a title cannot add headers")]
    [InlineData("Plain subject", "Plain subject")]
    [InlineData("Line one\r\nBcc: someone@example.com", "Line one Bcc: someone@example.com")]
    [InlineData("Line one\nLine two", "Line one Line two")]
    public void SubjectIsOneLine(string subject, string expected)
    {
        // Arrange
        // Act
        var message = new EmailMessage(["moderators@example.com"], subject, "Text");

        // Assert
        Assert.Equal(expected, message.Subject);
    }
}
