using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Aurore.Archetypes.Api.Tests.Integration.Support;

public sealed class KeycloakFixture : IAsyncLifetime
{
    private readonly Dictionary<string, string> _tokens = [];

    public WebApplicationFactory<Program> Factory { get; } = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder
        .ConfigureTestServices(services => services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            options.BackchannelHttpHandler = Ipv4Handler.Create())));

    public string TokenOf(string caller)
    {
        return _tokens[caller];
    }

    public async ValueTask InitializeAsync()
    {
        foreach (var caller in new[] { "alice", "bob" })
            _tokens[caller] = await KeycloakTokens.RequestAsync(caller, TestContext.Current.CancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await Factory.DisposeAsync();
    }
}
