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

/// <param name="Body">The text, up to 2048 characters.</param>
internal sealed record CreateCommentBody(string Body);

internal sealed class CreateCommentBodyValidator : RequestValidator<CreateCommentBody>
{
    public CreateCommentBodyValidator()
    {
        RuleFor(x => x.Body).NotEmpty().MaximumLength(BlogRules.CommentBodyMaxLength);
    }
}

internal sealed class CreateCommentEndpoint : MinimalEndpoint<CommentsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost(string.Empty, Handle)
            .RequireScopes(AppScopes.Write)
            .WithIdempotency();

        static async Task<Results<Created<CommentResponse>, ProblemHttpResult>> Handle(
            Guid postId,
            CreateCommentBody body,
            ClaimsPrincipal user,
            CreateComment feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new CreateCommentRequest(user.ToCaller().Subject, postId, body.Body), cancellationToken);

            return result.ToHttpResult(comment => TypedResults.Created($"/v1/posts/{postId}/comments", comment));
        }
    }
}
