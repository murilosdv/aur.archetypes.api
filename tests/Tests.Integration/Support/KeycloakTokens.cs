using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Aurore.Archetypes.Api.Tests.Integration.Support;

public static class KeycloakTokens
{
    private static readonly string _authority = Environment.GetEnvironmentVariable("TEST_KEYCLOAK_AUTHORITY") ?? "http://127.0.0.1:8080/realms/aurorelabs";
    private const string Secret = "local-dev-secret";
    private const string ComposeHint = "Start the dev-sbx infrastructure first (.dev-sbx/docker-compose.yaml: PostgreSQL, Keycloak), see docs/instructions/testing.md.";

    public static async Task<string> RequestAsync(string caller, CancellationToken cancellationToken)
    {
        using var http = new HttpClient(Ipv4Handler.Create()) { Timeout = TimeSpan.FromSeconds(30) };

        try
        {
            using var response = await http.PostAsync(
                $"{_authority}/protocol/openid-connect/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = $"caller-{caller}",
                    ["client_secret"] = Secret,
                }),
                cancellationToken);

            response.EnsureSuccessStatusCode();

            using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));

            return body.RootElement.GetProperty("access_token").GetString()!;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new InvalidOperationException($"Could not get a token for '{caller}' from Keycloak. {ComposeHint}", ex);
        }
    }
}
