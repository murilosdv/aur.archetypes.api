using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Aurore.Archetypes.Api.Tests.Integration.App.Api.OpenApi;

[Trait("Feature", "OpenAPI documents")]
public sealed class OpenApiDocumentsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact(DisplayName = "The document of the first version describes the blog, and a version that does not exist has none")]
    public async Task DocumentDescribesTheBlog()
    {
        // Arrange
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });

        // Act
        var v1 = await client.GetAsync("/openapi/v1.json", TestContext.Current.CancellationToken);
        var body = await v1.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var v9 = await client.GetAsync("/openapi/v9.json", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, v1.StatusCode);
        Assert.Contains("/v1/posts", body, StringComparison.Ordinal);
        Assert.Equal(HttpStatusCode.NotFound, v9.StatusCode);
    }
}
