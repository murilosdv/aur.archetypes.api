using System;
using Aurore.Archetypes.Api.Core.Clients.Email;
using Aurore.Archetypes.Api.Core.Settings;

namespace Aurore.Archetypes.Api.Features.Shared;

internal static class ModerationEmails
{
    public static EmailMessage CommentReported(EmailOptions options, CommentReportedDetails details)
    {
        var body = string.Join(Environment.NewLine,
            "A comment was reported.",
            string.Empty,
            $"Reported by: {details.Reporter}",
            $"Reason: {details.Reason}",
            string.Empty,
            $"Post: {details.PostTitle} ({details.PostSlug})",
            $"Post author: {details.PostAuthor}",
            string.Empty,
            $"Comment {details.CommentId} by {details.CommentAuthor}, written on {details.CommentCreatedAt:u}:",
            details.CommentBody);

        return new EmailMessage([.. options.ModeratorAddresses], $"Comment reported on \"{details.PostTitle}\"", body);
    }

    public static EmailMessage PostPublished(EmailOptions options, string title, string slug, string author)
    {
        var body = string.Join(Environment.NewLine,
            "A post was published.",
            string.Empty,
            $"Title: {title}",
            $"Slug: {slug}",
            $"Author: {author}");

        return new EmailMessage([.. options.ModeratorAddresses], $"Post published: \"{title}\"", body);
    }

    public static string Person(string handle, string displayName)
    {
        return $"{handle} ({displayName})";
    }
}

internal sealed record CommentReportedDetails(
    string Reporter,
    string Reason,
    string PostTitle,
    string PostSlug,
    string PostAuthor,
    Guid CommentId,
    string CommentAuthor,
    DateTimeOffset CommentCreatedAt,
    string CommentBody);
