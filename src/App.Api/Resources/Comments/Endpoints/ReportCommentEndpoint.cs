using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.App.Api.Resources.Shared;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Features.Comments;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.AspNetCore.RequestPipeline.Configuration;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Comments.Endpoints;

/// <param name="Reason">Why the comment should be looked at, up to 256 characters.</param>
internal sealed record ReportCommentBody(string Reason);

internal sealed class ReportCommentBodyValidator : RequestValidator<ReportCommentBody>
{
    public ReportCommentBodyValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(BlogRules.ReportReasonMaxLength);
    }
}

internal sealed class ReportCommentEndpoint : MinimalEndpoint<CommentsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("{commentId:guid}/report", Handle)
            .RequireScopes(AppScopes.Write)
            .WithIdempotency();

        static async Task<Results<Accepted<CommentReportResponse>, ProblemHttpResult>> Handle(
            Guid postId,
            Guid commentId,
            ReportCommentBody body,
            ClaimsPrincipal user,
            ReportComment feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new ReportCommentRequest(user.ToCaller().Subject, postId, commentId, body.Reason), cancellationToken);

            return result.ToHttpResult(report => TypedResults.Accepted((string?)null, report));
        }
    }
}
