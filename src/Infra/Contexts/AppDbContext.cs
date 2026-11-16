using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Foundation.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Infra.Contexts;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Post> Posts => Set<Post>();

    public DbSet<Comment> Comments => Set<Comment>();

    public DbSet<CommentReport> CommentReports => Set<CommentReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(IInfra).Assembly)
            .ApplyStandardConventions();
    }
}
