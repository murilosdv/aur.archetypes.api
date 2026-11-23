[Home](../../README.md)

# Using this as a template

This repository is meant to be copied. Do it in this order; build and run the tests after each step.

## 1. Copy and rename

| What | Where |
|---|---|
| The solution | `Archetypes.Api.slnx` (file name, and `Makefile`, `README.md`, `docker-compose.yaml` that mention it, and the `SOLUTION` argument of the `Dockerfile`) |
| The assembly and entry point | `AssemblyName` in `src/App.Api/App.Api.csproj` (`Archetypes.Api`) and `ENTRYPOINT ["dotnet", "Archetypes.Api.dll"]` in the `Dockerfile` |
| Root namespace | `Aurore.Archetypes.Api.*` in every project (`RootNamespace` and `InternalsVisibleTo` in the csprojs, the test projects' namespaces) |
| The service identity | `AppInformation` in `appsettings.json` (`Domain`, `Boundary`, `Component`, `Description`, `Maintainer`) and the staging and production files. These names go to telemetry, the OpenAPI title and the liveness answer. |
| Image, container and compose service names | `docker-compose.yaml` (`aur-archetypes-api`, `aur/archetypes-api`), `Dockerfile` |
| Database name | `Postgres:Database` in the three appsettings files, `Makefile` `CONNECTION`, [database](./database.md) |
| Company and authors | `Directory.Build.props`, `LICENSE`, `NOTICE.md` |
| Version | `<Version>` in `App.Api.csproj` (or `-p:Version=` in CI) |
| Docs and changelog | `README.md`, this `docs/` folder, `CHANGELOG.md` (start the history from scratch) |

If you use another prefix for telemetry and headers, call `PlatformNames.UsePrefix("acme")` as the first line of `Program.cs` ([constants](https://github.com/murilosdv/aur.foundation/blob/main/docs/core/constants.md#platformnames)).

## 2. Identity provider

The dev-sbx realm `aurorelabs` and its clients (`api`, `api-docs`, `caller-*`, `aur-archetypes-api`) belong to the dev-sbx. For a real service set `IdentityProvider` (authority, audience, scopes, provider kind), `ApiDocs:OAuthClientId` and the scopes in `AppScopes`; remove the `ApiKeys` client `local-tester` or replace it. See [authentication](./authentication.md).

## 3. Delete the playground

Follow [the playground](./playground.md): it lists every file, test and setting that exists only for the examples.

## 4. Replace the domain

The blog is the example business. To swap it:

1. Delete the entities (including `CommentReport`), configurations and `DbSet`s, the features, the endpoints and their tests, `BlogRules`, the blog errors in `AppErrors`, `bruno/Blog` and [the blog guide](./blog.md).
2. Generate a new initial migration for your model: `make db-reset` drops the database, removes the blog migration and creates `InitialDb` from your entities.
3. Add yours following [adding a feature](./adding-a-feature.md).
4. Keep the scaffolding: `Caller`, `AppScopes`, the unique violation registration in `AddAppDatabase`, the settings and their tests, and `bruno/Auth`. The email service (`IEmailSender`, `SmtpEmailSender`, `ModerationEmails`, the `Email` settings and the Mailpit of the dev-sbx) is kept as the example of a service shared by features; delete it, with `EmailOptions` and its validator, if you do not send email.

## 5. Check

```
dotnet build Archetypes.Api.slnx      # use your new name
make test-unit
make test
```

`AppSettingsFileTests` loads the real appsettings files strictly and checks that Production refuses development values, which is the quickest way to find a leftover.
