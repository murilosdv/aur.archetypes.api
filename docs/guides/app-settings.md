[Home](../../README.md)

# Application settings

`appsettings.json` is bound once at startup into `AppSettings`, validated, and passed as parameters to the `Add*`/`Use*` methods that need it.

```csharp
builder.LoadAppSettings(out var appSettings);   // bind + validate, or the app does not start
```

- **Strict binding:** an unknown key is an error, so a typo cannot fall back to a default.
- **Validation:** `AppSettingsValidator` composes the foundation packages' validators. Errors name the config key (`Telemetry:Otlp:Endpoint`).
- **Not in DI:** pass the section you need as a parameter; never inject `AppSettings`.
- **Exit codes:** invalid configuration exits with 78, a runtime crash with 70.

## Conventions

- Time is in **seconds**, and the name says so: `CacheSeconds`, `TimeoutSeconds`, `RetryAfterSeconds`.
- Switches are `Enabled`, health criticality is `Critical`, sizes carry the unit (`MaxRequestBodySizeBytes`).
- Group one level when the children belong together (`RequestPipeline`, `Telemetry:Otlp`); otherwise stay flat.

## Sections

| Section | Purpose |
| --- | --- |
| `AllowedHosts` | Framework host filtering. |
| `AppInformation` | Name, maintainer, locale; used by telemetry and the API docs. |
| `ApiDocs` | `OAuthClientId` for the docs login. |
| `ApiKeys` | Header name and API key clients. |
| `Email` | Mail server (`Host`, `Port`, `EnableSsl`, `Username`, `Password`, `From`) and `ModeratorAddresses`, who receives the reports and publication notices. Production refuses a local server and an unencrypted connection. |
| `HealthChecks` | `CacheSeconds`. |
| `Hosting` | `ShutdownTimeoutSeconds`. |
| `HttpDependencies` | Upstream services: URL, headers, auth, health check. |
| `IdentityProvider` | OIDC authority, audience, scopes. |
| `Logging` | Framework log levels. |
| `Pagination` | `DefaultPageSize`, `MaxPageSize` ([pagination](https://github.com/murilosdv/aur.foundation/blob/main/docs/core/pagination.md)). |
| `Postgres` | Npgsql connection keywords ([database](./database.md)). |
| `RequestPipeline` | CORS, forwarded headers, idempotency, Kestrel limits, rate limiting, compression, security headers, timeouts, privacy. |
| `Telemetry` | [Telemetry](https://github.com/murilosdv/aur.foundation/blob/main/docs/telemetry/README.md). |

## Environments

`appsettings.json` has the development defaults; `appsettings.Staging.json` and `appsettings.Production.json` override what must differ (hosts, URLs, logging, privacy; secrets come from the environment, named in the comments). The environments are DEV, STG and PRD, named Development, Staging and Production; a file for another one is `appsettings.<Environment>.json`.
In Production the app refuses to start while a development value is in effect (local URLs, plain HTTP, short API keys, localhost CORS origins).

Environment variables use `__` for nesting:

```
Telemetry__Otlp__Endpoint=http://otel-collector:4317
RequestPipeline__CorsPolicies__Default__AllowedOrigins__0=https://app.example.com
```

Array overrides merge by index; an empty array does not clear lower layers. Never commit real secrets: use environment variables or an encrypted file (SOPS + age).

## Add a setting

1. Add the property, with a default, to the options record.
2. Add the key to `appsettings.json`.
3. Add a rule to the validator.
4. Pass it as a parameter where it is used.
5. Run the tests: `AppSettingsFileTests` loads the real file strictly.
