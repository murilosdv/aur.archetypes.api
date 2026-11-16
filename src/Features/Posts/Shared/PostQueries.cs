using System.Linq;
using Aurore.Archetypes.Api.Core.Domain;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Posts.Shared;

internal static class PostQueries
{
    public static IQueryable<Post> VisibleTo(this IQueryable<Post> posts, Caller caller)
    {
        return posts.Where(x => x.Status == PostStatus.Published || caller.IsAdmin || x.Author.Subject == caller.Subject);
    }

    public static IQueryable<Post> TitleContaining(this IQueryable<Post> posts, string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return posts;

        var pattern = $"%{Escape(title)}%";

        return posts.Where(x => EF.Functions.ILike(x.Title, pattern, "\\"));
    }

    public static IQueryable<Post> WrittenBy(this IQueryable<Post> posts, string? handle)
    {
        return string.IsNullOrWhiteSpace(handle) ? posts : posts.Where(x => x.Author.Handle == handle);
    }

    public static IQueryable<PostSummaryResponse> ToSummaries(this IQueryable<Post> posts)
    {
        return posts.Select(x => new PostSummaryResponse
        {
            Id = x.Id,
            Slug = x.Slug,
            Title = x.Title,
            Status = x.Status,
            AuthorHandle = x.Author.Handle,
            PublishedAt = x.PublishedAt,
            CreatedAt = x.CreatedAt,
        });
    }

    // LIKE treats % and _ as wildcards; the user's text is searched for as it is.
    private static string Escape(string text)
    {
        return text.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");
    }
}
