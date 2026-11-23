# Aurore | Archetypes @ API

A complete, runnable **HTTP API** to copy when you start a new one. It is a small blog (authors, posts and comments) on PostgreSQL, secured with Keycloak tokens or API keys, observable over OpenTelemetry, containerized, and covered by tests against a real database and real tokens. It is built on [Aurore Foundation](https://github.com/murilosdv/aur.foundation) and shows how its pieces fit in a real service, so the code is the documentation and the guides here explain the choices.

## What you get

- **A domain with real rules:** author profiles, drafts and publishing, comments, who may do what by role and scope, soft delete, conflicts answered from unique indexes ([the blog](./docs/guides/blog.md)).
- **The layers and where code goes:** host, core, features, infrastructure ([architecture](./docs/guides/architecture.md)).
- **Authentication and authorization** with Keycloak bearer tokens and a read-only API key ([authentication](./docs/guides/authentication.md)).
- **Typed, strictly validated settings** with Staging and Production overrides ([settings](./docs/guides/app-settings.md)).
- **PostgreSQL with migrations** in a tools project ([database](./docs/guides/database.md)).
- **A Dockerfile and compose file** for the service and for the containerized tests ([running it](./docs/guides/deployment.md)).
- **Tests:** unit tests with no services, and integration tests on PostgreSQL and Keycloak ([testing](./docs/instructions/testing.md)).
- **A playground** of platform features (caching, rate limiting, idempotency, HTTP dependencies, …) that you delete when you do not need it ([the playground](./docs/guides/playground.md)).

```
src/      App.Api (host and endpoints), Core (settings, entities, errors), Features (use cases), Infra (database, HTTP clients)
tools/    Tools.Migrations (EF tools only)
bruno/    the Bruno collection: sign-in, the blog story, the playground examples
tests/    Tests (unit, no services), Tests.Integration (PostgreSQL, Keycloak, the whole application)
docs/     guides (how this service works), instructions (testing, conventions, changelog)
```

## Run it

**Prerequisites:** the .NET SDK (`global.json`), Docker, the shared **dev-sbx** running (`make sbx-up`: PostgreSQL, Keycloak and Mailpit; see [testing](./docs/instructions/testing.md#services)), and nothing else for the foundation packages: they come from nuget.org (see [installing](https://github.com/murilosdv/aur.foundation/blob/main/docs/installing.md)).

```
make db-reset      # drops and recreates the database from the model (InitialDb); see the database guide
dotnet run --project src/App.Api
```

The API documentation is at the Scalar page in Development, and the [Bruno](https://www.usebruno.com) collection in [`bruno`](./bruno/README.md) has requests for the blog and the playground. To run the service as a container, see [running it](./docs/guides/deployment.md).

## Test it

```
dotnet test --solution Archetypes.Api.slnx     # everything; needs the dev-sbx
make test-unit                                        # no services
make test                                             # in a Linux container, with a coverage report
```

## Make it yours

[Using this as a template](./docs/guides/using-as-a-template.md) lists what to rename, what to delete and what to replace.

## Guides

| Guide | About |
|---|---|
| [Architecture](./docs/guides/architecture.md) | The four projects, the dependency rule, the path of a request, where each kind of code goes. |
| [The blog](./docs/guides/blog.md) | The domain, who can do what, the endpoints, where the rules live. |
| [Adding a feature](./docs/guides/adding-a-feature.md) | A use case from entity to tests. |
| [Authentication](./docs/guides/authentication.md) | Tokens, roles, scopes, API key, the caller, the dev-sbx identities. |
| [Application settings](./docs/guides/app-settings.md) | Binding, validation, environments, adding a setting. |
| [Database](./docs/guides/database.md) | Settings, where things go, migrations. |
| [Running it](./docs/guides/deployment.md) | The image, container settings, the proxy, health, the dev-sbx. |
| [The playground](./docs/guides/playground.md) | The platform examples and how to delete them. |
| [Using this as a template](./docs/guides/using-as-a-template.md) | Rename, clean up, replace the domain. |
| [Testing](./docs/instructions/testing.md) | What is tested, the services, how to run. |
| [Code conventions](./docs/instructions/conventions.md) | Files and folders, names, style, comments, migrations in development, Bruno. |
| [Changelog](./docs/instructions/changelog.md) | How to write the changelog. |

The guides of the packages themselves (results, features, pagination, EF Core, authentication, request pipeline, endpoints, OpenAPI, telemetry) are in the [foundation docs](https://github.com/murilosdv/aur.foundation/tree/main/docs).

## Project

- [Changelog](./CHANGELOG.md)
- [Third-party notices](./NOTICE.md)
- [License](./LICENSE) ([MIT](https://mit-license.org))
