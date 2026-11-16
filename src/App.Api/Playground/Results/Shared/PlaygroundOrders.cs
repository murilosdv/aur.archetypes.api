using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Aurore.Archetypes.Api.App.Api.Playground.Results.Endpoints;
using Aurore.Foundation.Core.Results;
using Aurore.Foundation.Core.Validation;

namespace Aurore.Archetypes.Api.App.Api.Playground.Results.Shared;

internal static class PlaygroundOrders
{
    private static readonly ConcurrentDictionary<int, PlaygroundOrder> _orders = new()
    {
        [1] = new PlaygroundOrder(1, "ada@example.com", 2, "pending"),
        [2] = new PlaygroundOrder(2, "grace@example.com", 1, "shipped"),
    };

    private static int _lastId = 2;

    public static Result<PlaygroundOrder> Get(int id)
    {
        return _orders.TryGetValue(id, out var order) ? order : PlaygroundErrors.Orders.NotFound;
    }

    public static Result<PlaygroundOrder> Create(CreatePlaygroundOrderRequest request)
    {
        if (_orders.Values.Any(x => x.Email == request.Email))
            return PlaygroundErrors.Orders.DuplicateEmail;

        var order = new PlaygroundOrder(Interlocked.Increment(ref _lastId), request.Email, request.Quantity, "pending");
        _orders[order.Id] = order;

        return order;
    }

    public static Result<PlaygroundOrder> Cancel(int id)
    {
        var found = Get(id);

        if (found.IsFailure)
            return found.Error!;

        var validation = new CancelPlaygroundOrderValidator().Validate(found.Value);

        if (validation.IsValid is false)
            return validation.ToApplicationError(PlaygroundErrors.Orders.CannotCancel);

        var cancelled = found.Value with { Status = "cancelled" };
        _orders[id] = cancelled;

        return cancelled;
    }

    public static Result Delete(int id)
    {
        return _orders.TryRemove(id, out _) ? Result.Success : PlaygroundErrors.Orders.NotFound;
    }
}
