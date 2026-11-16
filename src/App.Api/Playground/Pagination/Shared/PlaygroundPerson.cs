using System;

namespace Aurore.Archetypes.Api.App.Api.Playground.Pagination.Shared;

internal sealed record PlaygroundPerson(int Id, string Name, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
