using Aurore.Archetypes.Api.Core.Clients.Email;
using Aurore.Archetypes.Api.Core.Clients.GitHub;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Archetypes.Api.Infra.Clients.Email;
using Aurore.Archetypes.Api.Infra.Clients.GitHub;
using Aurore.Archetypes.Api.Infra.Clients.RandomUser;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Archetypes.Api.Infra.Contexts.Configurations;
using Aurore.Foundation.AspNetCore.HttpDependencies.Configuration;
using Aurore.Foundation.EntityFrameworkCore.Configuration;
using Aurore.Foundation.EntityFrameworkCore.Features;
using Aurore.Foundation.EntityFrameworkCore.Postgres.Configuration;
using Aurore.Foundation.EntityFrameworkCore.Postgres.HealthChecks;
using Aurore.Foundation.EntityFrameworkCore.Postgres.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Aurore.Archetypes.Api.Infra.Configuration;

public static class InfraConfiguration
{
    public static IServiceCollection AddAppInfra(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddAppDatabase(appSettings.Postgres);
        services.AddSingleton(appSettings.Email);
        services.AddSingleton<IEmailSender, SmtpEmailSender>();
        services.AddPostgresHealthCheck(appSettings.Postgres);

        return services.AddAppHttpDependencies(appSettings);
    }

    public static IServiceCollection AddAppDatabase(this IServiceCollection services, PostgresOptions postgres)
    {
        return services
            .AddStandardDbContext<AppDbContext>(postgres)
            .AddUniqueViolationErrors(map => map
                .On(AuthorConfiguration.UniqueHandleIndex, AppErrors.Authors.HandleTaken)
                .On(CommentReportConfiguration.UniqueReporterIndex, AppErrors.Comments.AlreadyReported));
    }

    private static IServiceCollection AddAppHttpDependencies(this IServiceCollection services, AppSettings appSettings)
    {
        var dependencies = appSettings.HttpDependencies;

        // Typed, anonymous.
        services.AddStandardHttpDependency<IGitHubClient, GitHubClient>(HttpDependencyNames.GitHub, dependencies.GetRequired(HttpDependencyNames.GitHub));

        // Untyped, custom authentication handler.
        services.AddTransient<RandomUserSigningHandler>();
        services
            .AddStandardHttpDependency(HttpDependencyNames.RandomUser, dependencies.GetRequired(HttpDependencyNames.RandomUser))
            .AddHttpMessageHandler<RandomUserSigningHandler>();

        // Untyped, OAuth2 client credentials.
        var orders = dependencies.GetRequired(HttpDependencyNames.Orders);
        services
            .AddStandardHttpDependency(HttpDependencyNames.Orders, orders)
            .AddStandardClientCredentials(orders.ClientCredentials);

        // Untyped, static API key header.
        var inventory = dependencies.GetRequired(HttpDependencyNames.Inventory);
        services
            .AddStandardHttpDependency(HttpDependencyNames.Inventory, inventory)
            .AddStandardApiKey(inventory.ApiKey);

        return services;
    }
}
