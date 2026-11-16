using Aurore.Foundation.Core.Errors;

namespace Aurore.Archetypes.Api.Core.Errors;

public sealed class AppErrors : StandardErrors
{
    private AppErrors()
    {
    }

    public static class Authors
    {
        public static readonly ApplicationError ProfileRequired =
            ApplicationError.Conflict("author.profile_required", "Create your author profile first (PUT /v1/authors/me).");

        public static readonly ApplicationError HandleTaken =
            ApplicationError.Conflict("author.handle_taken", "That handle belongs to another author.");

        public static readonly ApplicationError HandleCannotChange =
            ApplicationError.Conflict("author.handle_cannot_change", "The handle of an author never changes.");
    }

    public static class Comments
    {
        public static readonly ApplicationError NotFound =
            ApplicationError.NotFound("comment.not_found", "There is no such comment on that post.");

        public static readonly ApplicationError AlreadyReported =
            ApplicationError.Conflict("comment.already_reported", "You already reported that comment.");
    }

    public static class Posts
    {
        public static readonly ApplicationError NotFound =
            ApplicationError.NotFound("post.not_found", "There is no such post.");

        public static readonly ApplicationError AlreadyPublished =
            ApplicationError.Conflict("post.already_published", "The post is already published.");

        public static readonly ApplicationError NotPublished =
            ApplicationError.Conflict("post.not_published", "Only published posts can be commented on.");

        public static readonly ApplicationError NotTheAuthor =
            ApplicationError.Forbidden("post.not_the_author", "Only the author of the post, or an admin, can do this.");
    }
}
