[Home](../../README.md)

# Architecture

Four projects, one direction of dependency, and one rule per layer. Knowing which layer a piece of code belongs to answers most "where do I put this?" questions.

## The projects

| Project | Holds | Does not hold |
|---|---|---|
| `src/App.Api` | The **host**: `Program.cs`, the resource groups and endpoints, the OpenAPI/Scalar setup, the loading of settings, `appsettings*.json`. | Business rules. |
| `src/Features` | The **use cases**: one file per feature (`CreatePost`, `PublishPost`, …) with its request, response, business validator and handler; what features share is in `Shared`. | HTTP, the database context's configuration. |
| `src/Infra` | **Infrastructure**: `AppDbContext`, entity configurations, clients of other services (`HttpClient` wrappers), their registration. | Rules. |
| `src/Core` | The **vocabulary** everyone shares: entities, `AppErrors`, constants and limits (`BlogRules`, `AppScopes`), `Caller`, the `AppSettings` class and its validator. | Anything that talks to something. |
| `tools/Tools.Migrations` | Only the EF tools: the migrations and the design-time factory. | Referenced by nothing. |
| `tests/Tests`, `tests/Tests.Integration` | Unit tests (no services) and integration tests (real PostgreSQL and Keycloak). | |

## Dependency rule

```
App.Api ──► Features ──► Infra ──► Core ──► Aurore.Foundation.*
   │            │                    ▲
   └────────────┴────────────────────┘
```

References only point **toward Core**. Core knows nothing of the layers above it; `Features` never references `App.Api`, so a feature cannot know it is called over HTTP and the same feature could run in a worker. Each project has a marker interface (`ICore`, `IFeatures`, `IInfra`) used to point at its assembly for scans (`AddFeaturesFrom<IFeatures>`, `AddStandardValidators`, `ApplyConfigurationsFromAssembly`).

## The path of a request

```
HTTP ─► request pipeline (context, errors, security headers, CORS) ─► authentication ─► rate limit, timeout, cache
     ─► endpoint (App.Api/Resources): validates the SHAPE of the body (RequestValidator), builds the Caller from the token
     ─► feature (Features): decorators telemetry → logging → validation (FeatureValidator) → ExecuteAsync
     ─► DbContext (Infra) ─► PostgreSQL
     ◄─ Result<T> ◄─ ToHttpResult() ◄─ 200 / 201 / 204 / problem+json
```

- The **endpoint** translates: HTTP in, a feature request out; a `Result` in, HTTP out. No `if` about the domain belongs there.
- The **feature** decides: rules, who may do what, state changes. It returns a `Result`; failures are `AppErrors` values, not exceptions.
- Unique conflicts that can race (the handle) are answered by the **unique violation decorator** from the unique indexes, not by check-then-insert.

The foundation guides explain each step: [results and errors](https://github.com/murilosdv/aur.foundation/blob/main/docs/core/results-and-errors.md), [features](https://github.com/murilosdv/aur.foundation/blob/main/docs/core/features.md), [request pipeline](https://github.com/murilosdv/aur.foundation/blob/main/docs/aspnetcore/request-pipeline.md), [minimal endpoints](https://github.com/murilosdv/aur.foundation/blob/main/docs/aspnetcore/minimal-endpoints.md).

## Startup

`Program.cs` is a list. Each line is a foundation method or one of the service's own (`AddApp…`, `MapApp…`, in `Configuration` folders):

| Line | Where |
|---|---|
| `LoadAppSettings` | `App.Api/Configuration/AppSettingsConfiguration.cs`: strict binding and validation ([settings](./app-settings.md)). |
| `AddAppScalar` / `MapAppScalar` | `App.Api/Configuration/ScalarConfiguration.cs`: the API documents and the Scalar page (not mapped in Production). |
| `AddAppValidators` | `App.Api/Configuration/ValidationConfiguration.cs`: scans the three assemblies for request and feature validators. |
| `AddAppFeatures` | `Features/Configuration/FeatureConfiguration.cs`: which decorators run for every feature. |
| `AddAppInfra` | `Infra/Configuration/InfraConfiguration.cs`: database, unique violation errors, health check, HTTP dependencies. |
| `MapAppResources` | `App.Api/Configuration/ResourcesConfiguration.cs`: maps the discovered endpoints. |

A bad configuration stops the process with exit code **78** before it serves anything; a crash while running exits with **70**.

## Where does it go?

| I am adding… | Put it in |
|---|---|
| An entity | `Core/Domain/Entities`, its table in `Infra/Contexts/Configurations`, then a migration |
| An error clients can see | `Core/Errors/AppErrors.cs` |
| A limit used by a validator and a column | `Core/Constants/BlogRules.cs` (inherits `FieldLengths`) |
| A use case | `Features/<Area>/<Feature>.cs`: request, response, validator and handler in one file |
| A type several features or endpoints use | A `Shared` folder next to them (`Features/Posts/Shared`, `Features/Shared`, `Resources/Shared`), see [conventions](../instructions/conventions.md) |
| Something that writes to the outside (email) | An interface in `Core/Clients/<Name>`, the implementation in `Infra/Clients/<Name>`, the words in `Features/Shared`, as `IEmailSender` and `ModerationEmails` ([the blog](./blog.md#a-service-shared-by-two-features-the-email-sender)) |
| A business rule that reads the database | A `FeatureValidator` next to the feature |
| An endpoint | `App.Api/Resources/<Area>/Endpoints`, one file each (+ its body record and `RequestValidator` in the same file) |
| A call to another service | An interface in `Core/Clients`, the implementation in `Infra/Clients`, settings in `HttpDependencies` |
| A setting | The record in `Core/Settings`, `appsettings.json`, the validator ([settings](./app-settings.md)) |
| A platform example | `App.Api/Playground`, never business ([the playground](./playground.md)) |
| The requests to try it | `bruno/<Area>`, one request per endpoint with its assertions |

See [adding a feature](./adding-a-feature.md) for the whole path, and the [code conventions](../instructions/conventions.md) for layout, names and style.
