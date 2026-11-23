[Home](../../README.md)

# Testing

Tests of the foundation packages live with them; see the foundation's [testing instructions](https://github.com/murilosdv/aur.foundation/blob/main/docs/instructions/testing.md). This page covers this service.

## What we test

Our own logic, never the framework or a package: a test that would pass with our code deleted is not worth keeping.

- Explicit `// Arrange`, `// Act`, `// Assert` sections (`// Arrange & Act` when they merge) and a `DisplayName` that reads as a rule.
- Scenario sets use `TheoryData` with `MemberData`, not long `InlineData` lists.
- A helper used only inside one test file stays in it; a fixture or stub several tests use is its own file ([conventions](./conventions.md)).
- Test folders mirror the source folders. `--filter-trait "Feature=Idempotency"` runs one feature.

| Project | Holds |
|---|---|
| `tests/Tests` | **Unit tests, no services:** the real settings files (they bind strictly and are valid) and our input rules (request validators, the caller). |
| `tests/Tests.Integration` | **Everything that needs a service:** the features on a real PostgreSQL (`Features/`), and the application through `WebApplicationFactory` with real Keycloak tokens (`App.Api/`: the blog over HTTP, its OpenAPI document, API keys, Keycloak tokens). `Support/` holds the fixtures and `TestDatabase`. |

## Services

These tests use the real PostgreSQL and Keycloak because a fake only proves the fake works (SQL translation, the claims Keycloak really writes, how roles and scopes reach the application). **The dev-sbx must be up**:
the infrastructure shared by every project of the machine (`.dev-sbx/docker-compose.yaml`). Start it with `make sbx-up` (PostgreSQL is the default database; `make sbx-down` removes its containers); it runs on its own, so nothing of it belongs to this repository. Without it the tests fail with a message saying what to start; they never skip, since a silent skip hides regressions.

- **PostgreSQL** on `127.0.0.1:5432`, `postgres` / `postgres`. Each test class creates a database `archetypes_<guid>` from the model and drops it afterwards.
- **Keycloak** at `http://127.0.0.1:8080`, realm `aurorelabs`, audience `api`, scopes `api.read` and `api.write`. Machine callers (client credentials, secret `local-dev-secret`): `caller-alice`, `caller-bob`, `caller-carol`, `caller-dave`, called alice, bob, carol and dave in the tests.
  alice, carol and dave are readers; bob is an admin and editor; carol never gets an author profile.

Use `127.0.0.1`, not `localhost`: Docker in WSL does not forward IPv6 and `localhost` resolves to `::1` first, so a connection can hang about twenty seconds. The Keycloak tests use an IPv4-only handler (`Ipv4Handler`) for this; do the same in any client test.
Email is never sent by the tests: `RecordingEmailSender` takes the place of the mail server, so a test reads what would have been sent and can make the server go down. Mailpit is only for running the service yourself.
WSL stops idle distributions after a few seconds, taking Docker with it; keep a WSL session open while the tests run.

## Running

```
dotnet test --solution Archetypes.Api.slnx
dotnet test --project tests/Tests/Tests.csproj                           # unit only, no services
dotnet test --project tests/Tests.Integration/Tests.Integration.csproj   # needs the dev-sbx
make test                                                                # everything in a Linux container, with coverage
```

`make` also has `test-local`, `test-unit`, `report` (opens the coverage report), `clean`, `sbx-up` / `sbx-down` for the dev-sbx and `sonar` (starts the dev-sbx with SonarQube, begin, build, tests with coverage, end, and leaves SonarQube running so the result can be read (`make sonar-down` in the dev-sbx removes it); the token is `SONAR_TOKEN` in `.env`). Results go to `artifacts/test-results`.
The container run is the `test` stage of the `Dockerfile`; it joins the `dev-sbx` network, so PostgreSQL is `sbx-postgres` (`TEST_POSTGRES_HOST`) and Keycloak is `sbx-keycloak:8080` (`TEST_KEYCLOAK_AUTHORITY`, and `IdentityProvider__AuthorityUrl` for the application): Keycloak has a fixed public URL (`http://127.0.0.1:8080`), so every token has the same issuer wherever it was asked; the container only uses the other name to reach Keycloak.
The coverage report is `artifacts/coverage-report/index.html`.

## What is left out of coverage

The number is about the code that matters, so three things are not counted, in the container report (`Dockerfile`) and in SonarQube (`SONAR_COVERAGE_EXCLUSIONS` in the `Makefile`):

- **The Playground** (`src/App.Api/Playground`): examples of platform features that you delete when you do not need them, so their tests are the ones you write when you keep one.
- **`Program.cs`:** the host start and its two crash handlers; the application tests start the real host.
- **Constants** (`AppPatterns`, `AppErrors`): `[ExcludeFromCodeCoverage]`, as in the foundation.

The generated migrations (`tools/Tools.Migrations/History`) are out of the analysis altogether. What needs a real server and has no test here (the SMTP sender) stays in the number on purpose.
