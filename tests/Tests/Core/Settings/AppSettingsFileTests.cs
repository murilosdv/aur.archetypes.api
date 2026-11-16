using System;
using System.Linq;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Archetypes.Api.Core.Validators;
using Aurore.Foundation.AspNetCore.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Aurore.Archetypes.Api.Tests.Core.Settings;

[Trait("Feature", "App settings")]
public sealed class AppSettingsFileTests
{
    [Fact(DisplayName = "appsettings.json binds without unknown keys and is valid in development")]
    public void DevelopmentSettingsAreValid()
    {
        // Arrange
        var settings = Load(null);
        var validator = new AppSettingsValidator(new TestEnvironment(Environments.Development));

        // Act
        var result = validator.Validate(settings);

        // Assert
        Assert.True(result.IsValid, string.Join(Environment.NewLine, result.Errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}")));
    }

    [Theory(DisplayName = "{0} overrides every local address of appsettings.json")]
    [InlineData("Production")]
    [InlineData("Staging")]
    public void DeployedEnvironmentsOverrideTheLocalAddresses(string environment)
    {
        // Arrange
        var settings = Load(environment);
        var validator = new AppSettingsValidator(new TestEnvironment(Environments.Production));

        // Act
        var result = validator.Validate(settings);

        // Assert
        Assert.DoesNotContain(result.Errors, x => x.ErrorMessage.Contains("local address"));
    }

    [Fact(DisplayName = "Production refuses the development API key until it is overridden")]
    public void ProductionRefusesTheDevelopmentApiKey()
    {
        // Arrange
        var settings = Load("Production");
        var validator = new AppSettingsValidator(new TestEnvironment(Environments.Production));

        // Act
        var result = validator.Validate(settings);

        // Assert
        Assert.Contains(result.Errors, x => x.ErrorMessage.Contains("Production API keys"));
    }

    [Fact(DisplayName = "Production turns privacy on, so it refuses to start until the encryption key is supplied")]
    public void ProductionRequiresThePrivacyKey()
    {
        // Arrange
        var settings = Load("Production");
        var validator = new AppSettingsValidator(new TestEnvironment(Environments.Production));

        // Act
        var result = validator.Validate(settings);

        // Assert
        Assert.True(settings.RequestPipeline.Privacy.Enabled);
        Assert.Contains(result.Errors, x => x.PropertyName.EndsWith("Privacy:Keys:Current"));
    }

    private static AppSettings Load(string? environment)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false);

        if (environment is not null)
            configuration.AddJsonFile($"appsettings.{environment}.json", optional: false);

        return configuration.Build().GetStrict<AppSettings>();
    }
}
