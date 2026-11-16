using System;
using System.Linq;
using Aurore.Foundation.Core.Pagination.Extensions;
using Aurore.Foundation.Core.Pagination.Models;
using Aurore.Foundation.Core.Results;

namespace Aurore.Archetypes.Api.App.Api.Playground.Pagination.Shared;

internal static class PlaygroundPeople
{
    private static readonly SortMap<PlaygroundPerson> _sort = SortMap.For<PlaygroundPerson>()
        .Allow(x => x.Name)
        .Allow(x => x.CreatedAt)
        .ById();

    private static readonly PlaygroundPerson[] _people =
    [
        .. Enumerable.Range(1, 45).Select(i => new PlaygroundPerson(
            i,
            $"Person {(char)('A' + (i % 26))}{i:00}",
            new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero).AddDays(i),
            new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero).AddDays(i))),
    ];

    public static Result<Page<PlaygroundPerson>> Search(
        SearchPlaygroundPeopleRequest request,
        PaginationOptions options)
    {
        var filtered = Filter(request.Name, request);

        return filtered.IsFailure ? filtered.Error! : filtered.Value.ToPage(request, options, _sort);
    }

    public static Result<CursorPage<PlaygroundPerson>> Stream(
        StreamPlaygroundPeopleRequest request,
        PaginationOptions options)
    {
        var filtered = Filter(request.Name, request);

        return filtered.IsFailure ? filtered.Error! : filtered.Value.ToCursorPage(request, options, _sort);
    }

    private static Result<IQueryable<PlaygroundPerson>> Filter(string? name, ITimestampFilter timestamps)
    {
        var query = _people.AsQueryable();

        if (string.IsNullOrWhiteSpace(name) is false)
            query = query.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        return query.WhereTimestamps(timestamps, x => x.CreatedAt, x => x.UpdatedAt);
    }
}
