using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Infra.Clients.RandomUser;
using Aurore.Archetypes.Api.Tests.Support;

namespace Aurore.Archetypes.Api.Tests.Infra.Clients.RandomUser;

[Trait("Feature", "Request signing")]
public sealed class RandomUserSigningHandlerTests
{
    private static readonly DateTimeOffset _now = new(2026, 11, 22, 12, 0, 0, TimeSpan.Zero);

    [Fact(DisplayName = "Every request carries the time it was signed at and a signature of 64 hexadecimal characters")]
    public async Task RequestCarriesTimestampAndSignature()
    {
        // Arrange
        var stub = new StubHttpHandler(HttpStatusCode.OK, "{}");
        using var http = CreateClient(stub, _now);

        // Act
        await http.GetAsync("api?results=1", TestContext.Current.CancellationToken);

        // Assert
        var headers = stub.Request!.Headers;
        Assert.Equal(_now.ToUnixTimeSeconds().ToString(), headers.GetValues("AUR-Timestamp").Single());
        Assert.Matches("^[0-9a-f]{64}$", headers.GetValues("AUR-Signature").Single());
    }

    [Fact(DisplayName = "The signature follows the path, the query and the time: a change of any of them changes it")]
    public async Task SignatureFollowsPathQueryAndTime()
    {
        // Arrange
        var signatures = new[]
        {
            await SignatureOf("api?results=1", _now),
            await SignatureOf("api?results=2", _now),
            await SignatureOf("other?results=1", _now),
            await SignatureOf("api?results=1", _now.AddSeconds(1)),
        };

        // Act
        var distinct = signatures.Distinct().Count();

        // Assert
        Assert.Equal(signatures.Length, distinct);
    }

    [Fact(DisplayName = "The same request at the same time always gets the same signature")]
    public async Task SameRequestGetsTheSameSignature()
    {
        // Arrange
        // Act
        var first = await SignatureOf("api?results=1", _now);
        var second = await SignatureOf("api?results=1", _now);

        // Assert
        Assert.Equal(first, second);
    }

    private static async Task<string> SignatureOf(string path, DateTimeOffset now)
    {
        var stub = new StubHttpHandler(HttpStatusCode.OK, "{}");
        using var http = CreateClient(stub, now);

        await http.GetAsync(path, TestContext.Current.CancellationToken);

        return stub.Request!.Headers.GetValues("AUR-Signature").Single();
    }

    private static HttpClient CreateClient(StubHttpHandler stub, DateTimeOffset now)
    {
        var signing = new RandomUserSigningHandler(new FixedTimeProvider(now)) { InnerHandler = stub };

        return new HttpClient(signing) { BaseAddress = new Uri("https://randomuser.test/") };
    }

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow()
        {
            return now;
        }
    }
}
