using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Foundation.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurore.Archetypes.Api.Infra.Contexts.Configurations;

internal sealed class AuthorConfiguration : EntityConfigurationBase<Author>
{
    public const string UniqueSubjectIndex = "ux_authors_subject";
    public const string UniqueHandleIndex = "ux_authors_handle";

    protected override void Extend(EntityTypeBuilder<Author> builder)
    {
        builder.Property(x => x.Subject).HasMaxLength(BlogRules.SubjectMaxLength);
        builder.Property(x => x.Handle).HasMaxLength(BlogRules.HandleMaxLength);
        builder.Property(x => x.DisplayName).HasMaxLength(BlogRules.DisplayNameMaxLength);

        builder.HasIndex(x => x.Subject).IsUnique().HasDatabaseName(UniqueSubjectIndex);
        builder.HasIndex(x => x.Handle).IsUnique().HasDatabaseName(UniqueHandleIndex);
    }
}
