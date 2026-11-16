[Home](../../README.md)

# Database

PostgreSQL through `Aurore.Foundation.EntityFrameworkCore.Postgres`; its [guide](https://github.com/murilosdv/aur.foundation/blob/main/docs/entity-framework/postgres.md) covers the conventions (snake case, the `app` schema, soft delete, timestamps, unique violations).
This page covers this service; the model is in the [blog guide](./blog.md).

## Settings

The `Postgres` section is a map of Npgsql keywords. The local values point at the database `archetypes-api` on `127.0.0.1`. Other environments set them through environment variables, the password above all:

```
Postgres__Host=db.internal.example.com
Postgres__Password=<secret>
```

Production refuses a local host.

## Where things go

| What | Where |
|---|---|
| `DbContext` | `src/Infra/Contexts/AppDbContext.cs` |
| Entity configurations (`EntityConfigurationBase<T>`) | `src/Infra/Contexts/Configurations`, found by `ApplyConfigurationsFromAssembly` |
| Registration | `AddAppDatabase` in `src/Infra/Configuration/InfraConfiguration.cs`: the context and the errors unique indexes answer with |
| Entities | `src/Core/Domain/Entities`, deriving from `Entity` or `SoftDeletableEntity` |
| Migrations | `tools/Tools.Migrations/History` (generated; style rules do not apply) |

## Migrations

The tool project exists only for the EF tools. The connection string is the first argument after `--`; the dev-sbx database is on `127.0.0.1:5432`, and the first update creates `archetypes-api`:

```
dotnet ef migrations add InitialDb --project tools/Tools.Migrations --output-dir History -- "Host=127.0.0.1;Database=archetypes-api;Username=postgres;Password=postgres"
dotnet ef database update --project tools/Tools.Migrations -- "<same connection string>"
dotnet ef migrations remove --project tools/Tools.Migrations -- "<same connection string>"
```

The Makefile wraps them (`CONNECTION` overrides the connection string): `make migration-add AddComments`, `db-update`, `migration-remove`, `migration-script`, `migration-check`, `db-drop`.

**While the model is in development** there is only one migration, `InitialDb`, and it is recreated every time: `make db-reset` drops the database, removes every migration, adds `InitialDb` from the current model and applies it. Once a version is released, switch to `make migration-add <Name>`.

The history table is `history.migrations`. The service does not apply migrations at startup; the tool does.

After changing an entity or configuration, add a migration. `make migration-check` (`dotnet ef migrations has-pending-model-changes`) fails when the model and the migrations disagree, and a build step can run it.
The tests create their databases from the model, not from the migrations.
