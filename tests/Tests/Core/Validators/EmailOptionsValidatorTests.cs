using System.Linq;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Archetypes.Api.Core.Validators;

namespace Aurore.Archetypes.Api.Tests.Core.Validators;

[Trait("Feature", "Email settings")]
public sealed class EmailOptionsValidatorTests
{
    private static EmailOptions Options()
    {
        return new EmailOptions
        {
            Host = "smtp.example.com",
            Port = 587,
            EnableSsl = true,
            From = "archetypes-api@example.com",
            ModeratorAddresses = ["moderators@example.com"],
        };
    }

    [Theory(DisplayName = "A complete mail server is valid in development and in production")]
    [InlineData(false)]
    [InlineData(true)]
    public void CompleteSettingsAreValid(bool isProduction)
    {
        // Arrange
        var validator = new EmailOptionsValidator(isProduction);

        // Act
        var result = validator.Validate(Options());

        // Assert
        Assert.True(result.IsValid, string.Join("; ", result.Errors.Select(x => x.ErrorMessage)));
    }

    [Fact(DisplayName = "The Mailpit of the dev-sbx is valid in development, without encryption and on this machine")]
    public void DevelopmentAcceptsALocalServerWithoutEncryption()
    {
        // Arrange
        var options = Options() with { Host = "127.0.0.1", Port = 1025, EnableSsl = false };

        // Act
        var result = new EmailOptionsValidator(isProduction: false).Validate(options);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory(DisplayName = "Production refuses a mail server on this machine or in the dev-sbx, and a connection that is not encrypted")]
    [InlineData("localhost", true)]
    [InlineData("127.0.0.1", true)]
    [InlineData("sbx-mailpit", true)]
    [InlineData("smtp.example.com", false)]
    public void ProductionRefusesTheDevelopmentServer(string host, bool refused)
    {
        // Arrange
        var options = Options() with { Host = host };

        // Act
        var result = new EmailOptionsValidator(isProduction: true).Validate(options);

        // Assert
        Assert.Equal(refused, result.Errors.Any(x => x.ErrorMessage.Contains("local address")));
    }

    [Fact(DisplayName = "Production refuses a connection that is not encrypted")]
    public void ProductionRequiresEncryption()
    {
        // Arrange
        var options = Options() with { EnableSsl = false };

        // Act
        var result = new EmailOptionsValidator(isProduction: true).Validate(options);

        // Assert
        Assert.Contains(result.Errors, x => x.ErrorMessage.Contains("encrypted"));
    }

    [Theory(DisplayName = "A port outside 1 to 65535, a sender that is not an address, and no moderators are all refused")]
    [InlineData(0, "archetypes-api@example.com", "moderators@example.com")]
    [InlineData(70000, "archetypes-api@example.com", "moderators@example.com")]
    [InlineData(587, "not-an-address", "moderators@example.com")]
    [InlineData(587, "archetypes-api@example.com", "not-an-address")]
    [InlineData(587, "archetypes-api@example.com", "")]
    public void InvalidFieldsAreRefused(int port, string from, string moderator)
    {
        // Arrange
        var options = Options() with { Port = port, From = from, ModeratorAddresses = [moderator] };

        // Act
        var result = new EmailOptionsValidator(isProduction: false).Validate(options);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Without any moderator address nobody would receive the reports, so it is refused")]
    public void ModeratorsAreRequired()
    {
        // Arrange
        var options = Options() with { ModeratorAddresses = [] };

        // Act
        var result = new EmailOptionsValidator(isProduction: false).Validate(options);

        // Assert
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(EmailOptions.ModeratorAddresses));
    }

    [Fact(DisplayName = "A Username needs a Password and the other way round")]
    public void CredentialsComeInPairs()
    {
        // Arrange
        var withoutPassword = Options() with { Username = "robot" };
        var withoutUsername = Options() with { Password = "secret" };
        var both = Options() with { Username = "robot", Password = "secret" };
        var validator = new EmailOptionsValidator(isProduction: false);

        // Act
        var withoutPasswordResult = validator.Validate(withoutPassword);
        var withoutUsernameResult = validator.Validate(withoutUsername);
        var bothResult = validator.Validate(both);

        // Assert
        Assert.Contains(withoutPasswordResult.Errors, x => x.PropertyName == nameof(EmailOptions.Password));
        Assert.Contains(withoutUsernameResult.Errors, x => x.PropertyName == nameof(EmailOptions.Username));
        Assert.True(bothResult.IsValid);
    }
}
