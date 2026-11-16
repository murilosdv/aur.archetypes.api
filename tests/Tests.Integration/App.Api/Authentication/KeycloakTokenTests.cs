using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Tests.Integration.Support;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Aurore.Archetypes.Api.Tests.Integration.App.Api.Authentication;

[Trait("Feature", "Keycloak tokens")]
public sealed class KeycloakTokenTests(KeycloakFixture keycloak) : IClassFixture<KeycloakFixture>
{
    public static TheoryData<string, string, HttpStatusCode> Access => new()
    {
        { "alice", "/v1/playground/auth/authenticated", HttpStatusCode.OK }, // any valid token
        { "alice", "/v1/playground/auth/scope", HttpStatusCode.OK }, // scope api.read
        { "alice", "/v1/playground/auth/role", HttpStatusCode.Forbidden }, // alice is a reader, not an admin
        { "bob", "/v1/playground/auth/role", HttpStatusCode.OK }, // the admin role of the realm
    };

    [Theory(DisplayName = "A real Keycloak token opens the endpoints its roles and scopes allow, and no others")]
    [MemberData(nameof(Access))]
    public async Task TokenOpensWhatItsRolesAndScopesAllow(string user, string path, HttpStatusCode expected)
    {
        // Arrange
        using var client = keycloak.Factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", keycloak.TokenOf(user));

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, response.StatusCode);
    }
}
