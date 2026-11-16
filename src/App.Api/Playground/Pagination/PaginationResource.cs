using Aurore.Archetypes.Api.App.Api.Playground.Shared;
namespace Aurore.Archetypes.Api.App.Api.Playground.Pagination;

internal sealed class PaginationResource() : PlaygroundResource("pagination", "Pagination")
{
    public override bool RequireAuthorization => false;
}
