using System;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Extensions;
using Aurore.Foundation.EntityFrameworkCore.Postgres.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aurore.Archetypes.Api.Tools.Migrations;

public sealed class MigrationContext : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = args.Length > 0 ? args[0] : null;

        if (connectionString.HasValue() is false)
            throw new InvalidOperationException("The connection string is missing. Pass it after '--': dotnet ef migrations add Name --project tools/Tools.Migrations -- \"Host=...;Database=...\"");

        var builder = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString, x =>
            {
                x.MigrationsHistoryTable("migrations", "history");
                x.MigrationsAssembly(typeof(MigrationContext).Assembly.FullName);
            });

        builder.UseSnakeCaseNpgsqlHistoryTable();

        return new AppDbContext(builder.Options);
    }
}
