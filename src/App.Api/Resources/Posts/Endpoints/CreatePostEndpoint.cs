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
using Aurore.Foundation.AspNetCore.RequestPipeline.Configuration;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Posts.Endpoints;

/// <param name="Title">The title, up to 128 characters.</param>
/// <param name="Body">The text, up to 32768 characters.</param>
internal sealed record CreatePostBody(string Title, string Body);

internal sealed class CreatePostBodyValidator : RequestValidator<CreatePostBody>
{
    public CreatePostBodyValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(BlogRules.TitleMaxLength);
        RuleFor(x => x.Body).NotEmpty().MaximumLength(BlogRules.PostBodyMaxLength);
    }
}

internal sealed class CreatePostEndpoint : MinimalEndpoint<PostsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost(string.Empty, Handle)
            .RequireAnyRole("editor", "admin")
            .RequireScopes(AppScopes.Write)
            .WithIdempotency();

        static async Task<Results<Created<PostResponse>, ProblemHttpResult>> Handle(
            CreatePostBody body,
            ClaimsPrincipal user,
            CreatePost feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new CreatePostRequest(user.ToCaller().Subject, body.Title, body.Body), cancellationToken);

            return result.ToHttpResult(post => TypedResults.Created($"/v1/posts/{post.Slug}", post));
        }
    }
}
