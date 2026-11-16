using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Clients.Email;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Archetypes.Api.Features.Shared;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Errors;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aurore.Archetypes.Api.Features.Comments;

public sealed record ReportCommentRequest(string Subject, Guid PostId, Guid CommentId, string Reason);

public sealed record CommentReportResponse(Guid Id, Guid CommentId, DateTimeOffset CreatedAt);

internal sealed class ReportCommentReporterValidator : FeatureValidator<ReportCommentRequest>
{
    public ReportCommentReporterValidator(AppDbContext db)
    {
        RuleFor(x => x.Subject)
            .MustAsync((subject, cancellationToken) => db.Authors.AnyAsync(x => x.Subject == subject, cancellationToken))
            .WithMessage(AppErrors.Authors.ProfileRequired.Message);
    }

    public override ApplicationError? Error => AppErrors.Authors.ProfileRequired;
}

public sealed class ReportComment(
    AppDbContext db,
    IEmailSender email,
    EmailOptions emailOptions,
    ILogger<ReportComment> logger)
    : Feature<ReportCommentRequest, CommentReportResponse>
{
    protected override async ValueTask<Result<CommentReportResponse>> ExecuteAsync(
        ReportCommentRequest request,
        CancellationToken cancellationToken)
    {
        var comment = await db.Comments
            .Where(x => x.Id == request.CommentId && x.PostId == request.PostId)
            .Select(x => new
            {
                x.Id,
                x.Body,
                x.CreatedAt,
                AuthorHandle = x.Author.Handle,
                AuthorName = x.Author.DisplayName,
                PostTitle = x.Post.Title,
                PostSlug = x.Post.Slug,
                PostAuthorHandle = x.Post.Author.Handle,
                PostAuthorName = x.Post.Author.DisplayName,
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (comment is null)
            return AppErrors.Comments.NotFound;

        var reporter = await db.Authors.SingleAsync(x => x.Subject == request.Subject, cancellationToken);
        var report = new CommentReport { Reason = request.Reason, CommentId = comment.Id, ReporterId = reporter.Id };

        db.CommentReports.Add(report);

        await db.SaveChangesAsync(cancellationToken);

        var details = new CommentReportedDetails(
            ModerationEmails.Person(reporter.Handle, reporter.DisplayName),
            request.Reason,
            comment.PostTitle,
            comment.PostSlug,
            ModerationEmails.Person(comment.PostAuthorHandle, comment.PostAuthorName),
            comment.Id,
            ModerationEmails.Person(comment.AuthorHandle, comment.AuthorName),
            comment.CreatedAt,
            comment.Body);

        await email.TrySendAsync(ModerationEmails.CommentReported(emailOptions, details), logger, cancellationToken);

        return new CommentReportResponse(report.Id, comment.Id, report.CreatedAt);
    }
}
