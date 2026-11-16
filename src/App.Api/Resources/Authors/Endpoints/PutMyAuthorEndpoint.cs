using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.App.Api.Resources.Shared;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Features.Authors;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Authors.Endpoints;

/// <param name="Handle">The public name, like a user name on a forum: letters and digits with single hyphens or underscores between them, in any case (my-name, my_name, MyName). It cannot change once chosen.</param>
/// <param name="DisplayName">The name shown next to posts and comments.</param>
internal sealed record PutAuthorBody(string Handle, string DisplayName);

internal sealed class PutAuthorBodyValidator : RequestValidator<PutAuthorBody>
{
    public PutAuthorBodyValidator()
    {
        RuleFor(x => x.Handle).NotEmpty().Length(BlogRules.HandleMinLength, BlogRules.HandleMaxLength).Matches(AppPatterns.Handle)
            .WithMessage("Use letters and digits, with single hyphens or underscores between them: my-name, my_name or MyName.");
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(BlogRules.DisplayNameMaxLength);
    }
}

internal sealed class PutMyAuthorEndpoint : MinimalEndpoint<AuthorsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPut("me", Handle)
            .RequireScopes(AppScopes.Write);

        static async Task<Results<Ok<AuthorResponse>, ProblemHttpResult>> Handle(
            PutAuthorBody body,
            ClaimsPrincipal user,
            UpsertAuthor feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new UpsertAuthorRequest(user.ToCaller().Subject, body.Handle, body.DisplayName), cancellationToken);

            return result.ToHttpResult();
        }
    }
}
