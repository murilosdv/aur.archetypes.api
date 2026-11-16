[Home](../../README.md)

# The playground

Examples of the platform, one resource per topic: authentication, caching, dependencies, diagnostics, idempotency, layers of conventions, pagination, rate limiting, results and errors, timeouts.
They live under `/v1/playground/{topic}`, are requested from the `Playground` folder of the [Bruno collection](../../bruno/README.md) and have nothing to do with the business (the blog).
This page lists everything that exists for them, so the project can be cleaned of them quickly.

## Delete

| Where | What |
|---|---|
| `src/App.Api/Playground` | The whole folder: resources, endpoints and their `Shared` folders. |
| `bruno/Playground` | The requests of the examples, a folder per topic. |
| `src/Core/Clients/GitHub` | `IGitHubClient` and `GitHubUser`, for the dependencies example (`Core/Clients/Email` is not an example: the blog uses it). |
| `src/Core/Constants/HttpDependencyNames.cs` | The names of the four upstream services. |
| `src/Infra/Clients/GitHub`, `src/Infra/Clients/RandomUser` | `GitHubClient` and `RandomUserSigningHandler` (an outbound request signing example). `Infra/Clients/Email` stays. |
| `tests/Tests.Integration/App.Api/Authentication` | `ApiKeyAuthenticationTests` and `KeycloakTokenTests`. |
| `tests/Tests.Integration/Support` | `KeycloakFixture`, used only by `KeycloakTokenTests`. |

Those tests check that the application accepts real Keycloak tokens and its API keys through the examples; the behavior itself is tested in the foundation. To keep the check, point them at the blog's endpoints.

## Edit

| File | Change |
|---|---|
| `src/App.Api/Configuration/ResourcesConfiguration.cs` | Remove the `using` of `Playground.Layers` and the layering filter. Keep `ProducesProblem(503)`. |
| `src/Infra/Configuration/InfraConfiguration.cs` | Remove `AddAppHttpDependencies`, its call in `AddAppInfra` (return `services` after the health check) and the `using`s it needed. |
| `tests/Tests/Core/Settings/AppSettingsFileTests.cs` | `ProductionRefusesTheDevelopmentDefaults` expects the two `HttpDependencies:Orders` keys to fail: remove those two lines. |
| `docs/guides/adding-a-feature.md` | The sentence about the `Playground` folder. |

## Settings (`appsettings.json`)

Used only by the examples:

| Section | Entry | Used by |
|---|---|---|
| `HttpDependencies` | `RandomUser`, `GitHub`, `Orders`, `Inventory` (the whole section can go; it defaults to empty) | The dependencies example. `RandomUser` also has the health check `/health/status` lists. |
| `RequestPipeline:OutputCache:Policies` | `short` | The caching example. |
| `RequestPipeline:RateLimiting:Policies` | `tests` | The rate limiting example. |

Unused by any endpoint and kept as examples of the settings: the rate limit policy `standard` and the timeout policy `long-running`.

Shared with the blog and the tests, so they stay: `ApiKeys` (the client `local-tester`), `IdentityProvider`, `Pagination`, `RequestPipeline:Idempotency`, `Postgres`, `HealthChecks`, `Telemetry` and the rest.

Afterwards, build and run `make test-unit` and `make test`. The dev-sbx Keycloak realm has nothing of the examples.
