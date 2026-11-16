using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Foundation.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurore.Archetypes.Api.Infra.Contexts.Configurations;

internal sealed class PostConfiguration : EntityConfigurationBase<Post>
{
    public const string UniqueSlugIndex = "ux_posts_slug";

    protected override void Extend(EntityTypeBuilder<Post> builder)
    {
        builder.Property(x => x.Slug).HasMaxLength(BlogRules.SlugMaxLength);
        builder.Property(x => x.Title).HasMaxLength(BlogRules.TitleMaxLength);
        builder.Property(x => x.Body).HasMaxLength(BlogRules.PostBodyMaxLength);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(BlogRules.StatusMaxLength);

        // The id is part of every slug, so this only guards against a bug.
        builder.HasIndex(x => x.Slug).IsUnique().HasDatabaseName(UniqueSlugIndex);
        builder.HasIndex(x => new { x.Status, x.PublishedAt });

        builder
            .HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
