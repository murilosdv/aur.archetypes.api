using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Foundation.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurore.Archetypes.Api.Infra.Contexts.Configurations;

internal sealed class CommentConfiguration : EntityConfigurationBase<Comment>
{
    protected override void Extend(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(x => x.Body).HasMaxLength(BlogRules.CommentBodyMaxLength);

        // Comments are listed by post, oldest or newest first.
        builder.HasIndex(x => new { x.PostId, x.CreatedAt });

        builder
            .HasOne(x => x.Post)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
