using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.App.Api.Resources.Shared;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Features.Posts;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Posts.Endpoints;

/// <param name="Title">The new title, up to 128 characters; leave it out to keep the current one.</param>
/// <param name="Body">The new text, up to 32768 characters; leave it out to keep the current one.</param>
internal sealed record PatchPostBody(string? Title, string? Body);

internal sealed class PatchPostBodyValidator : RequestValidator<PatchPostBody>
{
    public PatchPostBodyValidator()
    {
        RuleFor(x => x).Must(x => x.Title is not null || x.Body is not null).WithMessage("Send the title, the body or both.");
        RuleFor(x => x.Title).NotEmpty().MaximumLength(BlogRules.TitleMaxLength).When(x => x.Title is not null);
        RuleFor(x => x.Body).NotEmpty().MaximumLength(BlogRules.PostBodyMaxLength).When(x => x.Body is not null);
    }
}

internal sealed class PatchPostEndpoint : MinimalEndpoint<PostsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPatch("{id:guid}", Handle)
            .RequireAnyRole("editor", "admin")
            .RequireScopes(AppScopes.Write);

        static async Task<Results<Ok<PostResponse>, ProblemHttpResult>> Handle(
            Guid id,
            PatchPostBody body,
            ClaimsPrincipal user,
            UpdatePost feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new UpdatePostRequest(id, body.Title, body.Body, user.ToCaller()), cancellationToken);

            return result.ToHttpResult();
        }
    }
}
