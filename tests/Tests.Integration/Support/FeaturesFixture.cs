using System;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Clients.Email;
using Aurore.Archetypes.Api.Core.Domain;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Archetypes.Api.Features;
using Aurore.Archetypes.Api.Features.Authors;
using Aurore.Archetypes.Api.Features.Configuration;
using Aurore.Archetypes.Api.Features.Posts;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Infra.Configuration;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.AspNetCore.Configuration;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Pagination.Models;
using Aurore.Foundation.Core.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

namespace Aurore.Archetypes.Api.Tests.Integration.Support;

public sealed class FeaturesFixture : IAsyncLifetime
{
    public const string ModeratorAddress = "moderators@example.com";

    private TestDatabase? _database;

    public FakeTimeProvider Time { get; } = new(new DateTimeOffset(2026, 11, 11, 10, 0, 0, TimeSpan.Zero));

    public ServiceProvider Provider { get; private set; } = null!;

    public RecordingEmailSender Emails { get; } = new();

    public async ValueTask InitializeAsync()
    {
        _database = await TestDatabase.CreateAsync(TestContext.Current.CancellationToken);

        Provider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<TimeProvider>(Time)
            .AddSingleton(new PaginationOptions { DefaultPageSize = 10, MaxPageSize = 50 })
            .AddSingleton(new EmailOptions { Host = "localhost", From = "archetypes-api@example.com", ModeratorAddresses = [ModeratorAddress] })
            .AddSingleton<IEmailSender>(Emails)
            .AddStandardValidators([typeof(IFeatures).Assembly])
            .AddAppFeatures()
            .AddAppDatabase(_database.Options)
            .BuildServiceProvider();
    }

    public async ValueTask DisposeAsync()
    {
        await Provider.DisposeAsync();

        if (_database is not null)
            await _database.DisposeAsync();
    }

    public async Task<Result<TResponse>> RunAsync<TFeature, TRequest, TResponse>(TRequest request)
        where TFeature : Feature<TRequest, TResponse>
        where TRequest : notnull
    {
        await using var scope = Provider.CreateAsyncScope();

        return await scope.ServiceProvider.GetRequiredService<TFeature>().HandleAsync(request, TestContext.Current.CancellationToken);
    }

    public async Task<Result> RunAsync<TFeature, TRequest>(TRequest request)
        where TFeature : Feature<TRequest>
        where TRequest : notnull
    {
        await using var scope = Provider.CreateAsyncScope();

        return await scope.ServiceProvider.GetRequiredService<TFeature>().HandleAsync(request, TestContext.Current.CancellationToken);
    }

    public async Task<T> QueryAsync<T>(Func<AppDbContext, Task<T>> query)
    {
        await using var scope = Provider.CreateAsyncScope();

        return await query(scope.ServiceProvider.GetRequiredService<AppDbContext>());
    }

    public async Task<Author> NewAuthorAsync(string name = "writer")
    {
        var unique = Guid.NewGuid().ToString("N")[..8];
        var result = await RunAsync<UpsertAuthor, UpsertAuthorRequest, AuthorResponse>(new UpsertAuthorRequest($"subject-{unique}", $"{name}-{unique}", $"Writer {unique}"));

        return new Author(new Caller($"subject-{unique}", IsAdmin: false), result.Value);
    }

    public async Task<PostResponse> NewPostAsync(Author author, bool publish = false, string? title = null)
    {
        var created = await RunAsync<CreatePost, CreatePostRequest, PostResponse>(new CreatePostRequest(author.Caller.Subject, title ?? $"Title of {Guid.NewGuid():N}", "The text."));

        if (publish is false)
            return created.Value;

        return (await RunAsync<PublishPost, PublishPostRequest, PostResponse>(new PublishPostRequest(created.Value.Id, author.Caller))).Value;
    }

    public sealed record Author(Caller Caller, AuthorResponse Profile);
}
