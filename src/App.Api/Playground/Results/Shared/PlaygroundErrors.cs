using Aurore.Foundation.Core.Errors;

namespace Aurore.Archetypes.Api.App.Api.Playground.Results.Shared;

internal sealed class PlaygroundErrors : StandardErrors
{
    private PlaygroundErrors()
    {
    }

    public static class Orders
    {
        public static readonly ApplicationError NotFound =
            ApplicationError.NotFound("order.not_found", "The order was not found.");

        public static readonly ApplicationError DuplicateEmail =
            ApplicationError.Conflict("order.duplicate_email", "An order for this email already exists.");

        public static readonly ApplicationError CannotCancel =
            ApplicationError.Conflict("order.cannot_cancel", "The order can no longer be cancelled.");
    }
}
