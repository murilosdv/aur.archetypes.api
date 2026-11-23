[Home](../../README.md)

# Running it

## The image

The `Dockerfile` builds with the SDK image and runs on the ASP.NET image as the non-root user `app`, on port 8080. The build context is this repository. The foundation packages come from nuget.org, so the build needs no credentials. The solution file is the `SOLUTION` build argument. `Dockerfile.dockerignore` keeps the rest out of the context.

```
docker compose build
```

## Settings

The image has no configuration of its own: `appsettings.json` has only what is the same everywhere, and everything that differs (the `{{NAME}}` placeholders of the Staging and Production files) is an environment variable (`__` for nesting). The ones that matter in a container:

| Variable | Why |
|---|---|
| `Postgres__Host`, `Postgres__Database`, `Postgres__Username`, `Postgres__Password` | Where the database is. The password only from the environment or a secret store. |
| `IdentityProvider__AuthorityUrl` | Where the service fetches the identity provider's metadata and keys. The tokens name the provider by its public URL (the issuer, fixed in Keycloak); in the dev-sbx a container uses `http://sbx-keycloak:8080/realms/aurorelabs`. |
| `RequestPipeline__ForwardedHeaders__KnownProxies__0` or `__KnownNetworks__0` | The proxy whose `X-Forwarded-*` headers are trusted. Without it the client address is the proxy's. |
| `RequestPipeline__Privacy__Keys__Current` | The key that encrypts personal data in logs and traces ([privacy](https://github.com/murilosdv/aur.foundation/blob/main/docs/core/privacy.md)). Required in Production. |
| `Email__Host`, `Email__Port`, `Email__EnableSsl`, `Email__Username`, `Email__Password`, `Email__From`, `Email__ModeratorAddresses__0` | The mail server and who receives the reports. The password only from the environment or a secret store; Production refuses a local server and an unencrypted connection. In the dev-sbx: `Email__Host=sbx-mailpit`. |
| `Telemetry__Otlp__Endpoint` | The collector. |
| `ASPNETCORE_ENVIRONMENT` | `Production` makes the service refuse to start while a development value is in effect. |

The schema is not applied at startup; the migrations tool does it ([database](./database.md)).

## What sits in front

The service speaks plain HTTP and expects a reverse proxy (nginx) to terminate TLS, set the forwarded headers and own CORS and compression limits. The proxy must pass these headers, and the service must trust it (the setting above):

```
proxy_set_header Host $host;
proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
proxy_set_header X-Forwarded-Proto $scheme;
proxy_set_header X-Forwarded-Host $host;
```

## Health and stopping

- `/health/live` for liveness, `/health/ready` for readiness (PostgreSQL is critical), `/health/status` for detail (needs a caller). The ASP.NET image has no `curl`; a container health check can use bash: `bash -c 'exec 3<>/dev/tcp/localhost/8080 && ...'`.
- On stop, the service waits `Hosting:ShutdownTimeoutSeconds` (10) for requests in flight, so the orchestrator must allow at least that (`stop_grace_period: 15s`).

## The dev-sbx

On a development machine the service runs against the dev-sbx (`.dev-sbx` next to the repositories): PostgreSQL, Keycloak and Mailpit (a mail server that keeps what it receives), shared by every project on the network `dev-sbx`.
This repository's `docker-compose.yaml` defines `aur-archetypes-api`, published at `http://127.0.0.1:5000`, with tokens from `http://127.0.0.1:8080`: `docker compose up -d --build`.
