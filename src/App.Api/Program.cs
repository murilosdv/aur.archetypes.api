using System;
using Aurore.Archetypes.Api.App.Api.Configuration;
using Aurore.Archetypes.Api.Features.Configuration;
using Aurore.Archetypes.Api.Infra.Configuration;
using Aurore.Foundation.AspNetCore.Authentication.Configuration;
using Aurore.Foundation.AspNetCore.Configuration;
using Aurore.Foundation.AspNetCore.HealthChecks.Configuration;
using Aurore.Foundation.AspNetCore.MinimalApis.Configuration;
using Aurore.Foundation.AspNetCore.RequestPipeline.Configuration;
using Aurore.Foundation.Core.Application;
using Aurore.Foundation.Core.Application.Constants;
using Aurore.Foundation.Telemetry.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

WebApplication app;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder
        .ConfigureStandardSerializers()
        .LoadAppSettings(out var appSettings)
        .ConfigureStandardHosting(appSettings.Hosting)
        .AddStandardTelemetry(appSettings.AppInformation, appSettings.Telemetry)
        .AddStandardMinimalEndpoints<Program>(out var endpoints)
        .AddAppScalar(appSettings, endpoints)
        .AddStandardRequestPipeline(appSettings.RequestPipeline);

    AppDiagnostics.Initialize(appSettings.AppInformation.ActivitySource);

    builder.Services
        .AddSingleton(TimeProvider.System)
        .AddSingleton(appSettings.Pagination)
        .AddStandardApiVersioning(endpoints)
        .AddStandardAuthConfiguration(appSettings.IdentityProvider, appSettings.ApiKeys)
        .AddStandardHealthChecks(appSettings.AppInformation, appSettings.HealthChecks, appSettings.HttpDependencies)
        .AddAppValidators(appSettings)
        .AddAppFeatures()
        .AddAppInfra(appSettings);

    app = builder.Build();

    app
        .UseStandardRequestPipeline(appSettings.RequestPipeline)
        .UseAuthentication()
        .UseAuthorization()
        .UseStandardEndpointPolicies();

    app.MapAppScalar(appSettings);

    app.MapStandardHealthChecks(appSettings.AppInformation);

    app.MapAppResources(endpoints);
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    await Console.Error.WriteLineAsync($"Application failed to start: {ex}");

    return ExitCodes.ConfigurationError;
}

try
{
    app.Logger.LogInformation("Application is starting up...");

    await app.RunAsync();

    return ExitCodes.Success;
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    await Console.Error.WriteLineAsync($"Application stopped unexpectedly: {ex}");

    return ExitCodes.RuntimeError;
}
