using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Foundation.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aurore.Archetypes.Api.Infra.Contexts.Configurations;

internal sealed class CommentReportConfiguration : EntityConfigurationBase<CommentReport>
{
    public const string UniqueReporterIndex = "ux_comment_reports_comment_reporter";

    protected override void Extend(EntityTypeBuilder<CommentReport> builder)
    {
        builder.Property(x => x.Reason).HasMaxLength(BlogRules.ReportReasonMaxLength);

        // An author reports a comment once, even when two requests arrive together.
        builder.HasIndex(x => new { x.CommentId, x.ReporterId }).IsUnique().HasDatabaseName(UniqueReporterIndex);

        builder
            .HasOne(x => x.Comment)
            .WithMany()
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Reporter)
            .WithMany()
            .HasForeignKey(x => x.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
