using System.Globalization;
using Aurore.Archetypes.Api.Core;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Archetypes.Api.Features;
using Aurore.Foundation.AspNetCore.Configuration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Aurore.Archetypes.Api.App.Api.Configuration;

internal static class ValidationConfiguration
{
    internal static IServiceCollection AddAppValidators(this IServiceCollection services, AppSettings appSettings)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(appSettings.AppInformation.Locale);

        return services.AddStandardValidators(
            [typeof(Program).Assembly, typeof(ICore).Assembly, typeof(IFeatures).Assembly],
            includeInternalTypes: true);
    }
}
