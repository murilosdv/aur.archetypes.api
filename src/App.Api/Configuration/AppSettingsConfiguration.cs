using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Archetypes.Api.Core.Validators;
using Aurore.Foundation.AspNetCore.Configuration;
using FluentValidation;
using Microsoft.Extensions.Hosting;

namespace Aurore.Archetypes.Api.App.Api.Configuration;

internal static class AppSettingsConfiguration
{
    internal static IHostApplicationBuilder LoadAppSettings(
        this IHostApplicationBuilder builder,
        out AppSettings appSettings)
    {
        appSettings = builder.Configuration.GetStrict<AppSettings>();

        new AppSettingsValidator(builder.Environment).ValidateAndThrow(appSettings);

        return builder;
    }
}
