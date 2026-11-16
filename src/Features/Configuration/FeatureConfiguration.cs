using Aurore.Foundation.Core.Features.Configuration;
using Aurore.Foundation.EntityFrameworkCore.Postgres.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Aurore.Archetypes.Api.Features.Configuration;

public static class FeatureConfiguration
{
    public static IServiceCollection AddAppFeatures(this IServiceCollection services)
    {
        // Opt-in: what is enabled here applies to all features; a feature adds or removes one with the With/Without attributes.
        return services.AddFeaturesFrom<IFeatures>(features => features
            .WithTelemetry()
            .WithLogging()
            .WithValidation()
            .WithUniqueViolation());
    }
}
