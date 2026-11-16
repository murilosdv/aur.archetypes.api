using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.Core.Extensions;
using Aurore.Foundation.OpenApi.Extensions;
using Aurore.Foundation.OpenApi.Scalar.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace Aurore.Archetypes.Api.App.Api.Configuration;

internal static class ScalarConfiguration
{
    internal static IHostApplicationBuilder AddAppScalar(
        this IHostApplicationBuilder builder,
        AppSettings appSettings,
        MinimalEndpointCatalog endpoints)
    {
        if (builder.Environment.IsProduction())
            return builder;

        builder.ConfigureStandardScalarDocuments(endpoints.Versions);

        foreach (var version in endpoints.Versions)
        {
            builder.Services
                .AddOpenApi(OpenApiExtensions.DocumentName(version.Number), options => options
                    .AddStandardDocumentInfo(version, appSettings.AppInformation)
                    .AddStandardJwtBearerSecurity()
                    .AddStandardApiKeySecurity(appSettings.ApiKeys)
                    .AddStandardOidcSecurity(appSettings.IdentityProvider)
                    .AddStandardOAuth2Security(appSettings.IdentityProvider)
                    .AddStandardCamelCaseQueryParameters()
                    .AddStandardCorrelationIdHeader()
                    .AddStandardIdempotencyKeyHeader());
        }

        return builder;
    }

    internal static WebApplication MapAppScalar(this WebApplication app, AppSettings appSettings)
    {
        if (app.Environment.IsProduction())
            return app;

        app.MapOpenApi();

        app.MapScalarApiReference(options => options
            .WithTitle(appSettings.AppInformation.Component.ToTitleCase())
            .WithTheme(ScalarTheme.Default)
            .WithDefaultHttpClient(ScalarTarget.Shell, ScalarClient.Curl)
            .WithStandardAuthentication(appSettings.IdentityProvider, appSettings.ApiDocs));

        return app;
    }
}
