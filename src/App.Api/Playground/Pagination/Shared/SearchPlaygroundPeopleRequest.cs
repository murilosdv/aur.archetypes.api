using Aurore.Foundation.Core.Pagination.Models;

namespace Aurore.Archetypes.Api.App.Api.Playground.Pagination.Shared;

internal sealed class SearchPlaygroundPeopleRequest : TimestampPageRequest
{
    public string? Name { get; init; }
}
