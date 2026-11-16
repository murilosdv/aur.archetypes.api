[Home](../../README.md)

# Adding a feature

The path of a new use case through this service, with the blog as the example ([the blog](./blog.md)).

1. **Model.** A new entity goes in `src/Core/Domain/Entities`, deriving from `Entity` (timestamps) or `SoftDeletableEntity` (plus soft delete). Add its `DbSet` to `AppDbContext` and a configuration in `src/Infra/Contexts/Configurations`
   (`EntityConfigurationBase<T>`: lengths, indexes, relationships). Name unique indexes with constants.
2. **Migration.** While the model is in development there is one migration: `make db-reset` recreates `InitialDb` from the model. Read it: snake case names, the indexes you expect ([database](./database.md)). After a release, `make migration-add <Name>`.
3. **Errors.** Expected failures are `ApplicationError` values in `AppErrors` (`src/Core/Errors`), one line each, with a stable code. A unique index clients can collide with gets its error registered in `AddAppDatabase`.
4. **Feature.** One file per use case in `src/Features/<Area>` with its `<Name>Request`, its response (`<Resource>Response`) and the logic in `ExecuteAsync`, returning a `Result`; what other features share goes in a `Shared` folder ([conventions](../instructions/conventions.md)). A feature that tells someone something uses a service from `Core/Clients` (`IEmailSender`), not the mail library. Rules that read the database are `FeatureValidator`s of the request, so the feature only sees valid requests.
   Lists use `ToPageAsync` or `ToCursorPageAsync` with a `SortMap` and project with object initializers ([pagination](https://github.com/murilosdv/aur.foundation/blob/main/docs/core/pagination.md)).
   Features are found by their base class, with the decorators chosen in `AddAppFeatures`.
5. **Endpoint.** One file per endpoint in `src/App.Api/Resources/<Area>/Endpoints` (the group is `<Area>Resource` next to it), with its body record and `RequestValidator` in the same file: validate the body shape with it, take the caller from the token (`ToCaller()`), call the feature, return `ToHttpResult()`.
   State who may call it (`RequireRoles`, `RequireScopes`) and what it documents (`ProducesPage`, `ProducesCursorPage`). **No business rule goes in an endpoint.**
6. **Tests.** Input rules of the request in `tests/Tests` (no services). The feature on a real PostgreSQL in `tests/Tests.Integration/Features`, a case per rule plus the failures. The HTTP side in `tests/Tests.Integration/App.Api`
   (who may call it, status codes, answer shapes) with real tokens ([testing](../instructions/testing.md)).
7. **Bruno.** The request and its assertions in `bruno/<Area>`, so the collection always matches the API.
8. **Docs.** What it does for a client, in the guide of its area.

`App.Api/Playground` holds platform examples only, never business ([the playground](./playground.md)).
