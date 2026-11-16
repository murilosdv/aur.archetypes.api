using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Clients.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aurore.Archetypes.Api.Tests.Integration.Support;

public sealed class BlogFixture : IAsyncLifetime
{
    private static readonly string[] _users = ["alice", "bob", "carol", "dave"];

    private readonly Dictionary<string, string> _tokens = [];
    private readonly List<string> _variables = [];
    private TestDatabase? _database;

    public WebApplicationFactory<Program> Factory { get; private set; } = null!;

    public RecordingEmailSender Emails { get; } = new();

    public string HandleOf(string user)
    {
        return $"{user}-{_suffix}";
    }

    private readonly string _suffix = Guid.NewGuid().ToString("N")[..6];

    public async ValueTask InitializeAsync()
    {
        _database = await TestDatabase.CreateAsync(TestContext.Current.CancellationToken);

        foreach (var (key, value) in _database.Options)
            SetVariable($"Postgres__{key}", value);

        foreach (var user in _users)
            _tokens[user] = await KeycloakTokens.RequestAsync(user, TestContext.Current.CancellationToken);

        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder
            .ConfigureTestServices(services =>
            {
                services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options => options.BackchannelHttpHandler = Ipv4Handler.Create());
                services.RemoveAll<IEmailSender>();
                services.AddSingleton<IEmailSender>(Emails);
            }));

        foreach (var user in new[] { "alice", "bob" })
        {
            using var response = await SendAsync(user, HttpMethod.Put, "/v1/authors/me", new { handle = HandleOf(user), displayName = char.ToUpper(user[0]) + user[1..] });
            response.EnsureSuccessStatusCode();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Factory is not null)
            await Factory.DisposeAsync();

        foreach (var name in _variables)
            Environment.SetEnvironmentVariable(name, null);

        if (_database is not null)
            await _database.DisposeAsync();
    }

    public async Task<HttpResponseMessage> SendAsync(
        string? user,
        HttpMethod method,
        string path,
        object? body = null,
        string? idempotencyKey = null)
    {
        using var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });
        using var request = new HttpRequestMessage(method, path);

        if (user is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokens[user]);

        if (idempotencyKey is not null)
            request.Headers.Add("Idempotency-Key", idempotencyKey);

        if (body is not null)
            request.Content = JsonContent.Create(body);

        return await client.SendAsync(request, TestContext.Current.CancellationToken);
    }

    public async Task<HttpResponseMessage> SendWithApiKeyAsync(HttpMethod method, string path, object? body = null)
    {
        using var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });
        using var request = new HttpRequestMessage(method, path);

        request.Headers.Add("X-Api-Key", "local-dev-api-key");

        if (body is not null)
            request.Content = JsonContent.Create(body);

        return await client.SendAsync(request, TestContext.Current.CancellationToken);
    }

    public static async Task<JsonDocument> JsonAsync(HttpResponseMessage response)
    {
        return JsonDocument.Parse(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }

    public async Task<(Guid Id, string Slug)> NewPostAsync(bool publish)
    {
        using var created = await SendAsync("bob", HttpMethod.Post, "/v1/posts", new { title = $"Title of {Guid.NewGuid():N}", body = "The text." });
        created.EnsureSuccessStatusCode();

        using var json = await JsonAsync(created);
        var id = json.RootElement.GetProperty("id").GetGuid();
        var slug = json.RootElement.GetProperty("slug").GetString()!;

        if (publish)
        {
            using var published = await SendAsync("bob", HttpMethod.Post, $"/v1/posts/{id}/publish");
            published.EnsureSuccessStatusCode();
        }

        return (id, slug);
    }

    private void SetVariable(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value);
        _variables.Add(name);
    }
}

[CollectionDefinition(Name)]
public sealed class BlogCollection : ICollectionFixture<BlogFixture>
{
    public const string Name = "Blog end to end";
}
