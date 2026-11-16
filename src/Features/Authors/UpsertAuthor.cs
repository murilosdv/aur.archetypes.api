using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Authors;

public sealed record UpsertAuthorRequest(string Subject, string Handle, string DisplayName);

public sealed record AuthorResponse(Guid Id, string Handle, string DisplayName, DateTimeOffset CreatedAt);

public sealed class UpsertAuthor(AppDbContext db) : Feature<UpsertAuthorRequest, AuthorResponse>
{
    protected override async ValueTask<Result<AuthorResponse>> ExecuteAsync(
        UpsertAuthorRequest request,
        CancellationToken cancellationToken)
    {
        var author = await db.Authors.AsTracking().SingleOrDefaultAsync(x => x.Subject == request.Subject, cancellationToken);

        switch (author)
        {
            case null:
                author = new Author { Subject = request.Subject, Handle = request.Handle, DisplayName = request.DisplayName };
                db.Authors.Add(author);
                break;
            case { Handle: var current } when current != request.Handle:
                return AppErrors.Authors.HandleCannotChange;
            default:
                author.DisplayName = request.DisplayName;
                break;
        }

        await db.SaveChangesAsync(cancellationToken);

        return new AuthorResponse(author.Id, author.Handle, author.DisplayName, author.CreatedAt);
    }
}
