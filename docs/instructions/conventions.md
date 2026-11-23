[Home](../../README.md)

# Code conventions

How code is laid out, named and written in this service. The analyzers (`.editorconfig`) catch some of it; the rest is on whoever writes the code. When a rule does not fit a case, ask before bending it.

## Files and folders

The rule is **cohesion**: what exists only for one thing lives in that thing's file; what other code uses stands alone. A type used by one file is not a file of its own; a type used by two is.

| Where | Layout |
|---|---|
| `App.Api/Resources/<Area>` | `<Area>Resource.cs` is the route group, always its own file. `Endpoints/<Name>Endpoint.cs` holds one endpoint with its body record and its `RequestValidator`. |
| `Features/<Area>` | `<Feature>.cs` holds the request, the response, the business validator and the handler. |
| `Shared` | What several endpoints or features use: `Resources/Shared`, `Features/Posts/Shared` (inside an area), `Features/Shared` (across areas), `Playground/<Topic>/Shared`. The root of `Endpoints` or of a feature area holds only endpoints or features. |
| `tests` | Mirror the source folders. A helper used only inside one test file stays in it; a fixture or stub that several tests use is its own file. |

Namespaces follow the folders (`...Resources.Posts.Endpoints`, `...Features.Posts.Shared`).

## Project files, Makefiles and Dockerfiles

The same order everywhere, top-down, so any file of these kinds reads like the others.

- **Project file (`.csproj`):** one `PropertyGroup` (`Version` when the project has one, `AssemblyName`, `RootNamespace`, then the rest), then one `ItemGroup` each for `FrameworkReference`, `InternalsVisibleTo`, `PackageReference`, `ProjectReference`, files (`Content`) and `Using`. Items inside a group are in alphabetical order. A comment only where the reason is not obvious.
- **Makefile:** `-include` first, `export` next, then the variables grouped under a one-line comment (project, dev-sbx, SonarQube, migrations), then `.PHONY`, then the targets in sections headed `## Section ##` (dev-sbx, SonarQube, tests, clean, migrations, database). A command that repeats is a variable.
- **Dockerfile:** `syntax` line and `ARG` first, then the stages in order, each headed `## Stage: what it does ##` (restore, build, test, publish, runtime). The test stage does not run as root.

## Names

- A feature's input ends in `Request` and its output in `Response`: `CreatePostRequest`, `PostResponse`. Items of a page are `PostSummaryResponse`, never `PostItem`. The request an endpoint binds from the body is `CreatePostBody`; a filter bound from the query and wrapped with the caller is `SearchPostsFilter` inside `SearchPostsRequest`.
- Types of the foundation that are base classes or wrappers (`TimestampCursorRequest`, `Page<T>`) keep their own names.
- A request never carries what the client must not choose (the subject comes from the token and is added by the endpoint).

## Style

- **Pattern matching wherever it applies:** a `switch` for an `if` / `else if` / `else` on one value, and `is` / `is not` against constants (`Count is 0`, `status is not PostStatus.Published`). Not possible against `static readonly` values or inside an EF expression tree. A negative test is `is false`, never `!`: `Contains(x) is false`. SonarQube rule S1125 (unnecessary Boolean literal) would flag these, so it is off in `.editorconfig`.
- **No braces around a body of one line** (`if`, `else`, `foreach`, `while`); braces when it spans lines. Keep the braces on every branch of an `if` / `else` chain that has a braced branch. IDE0011 only asks for missing braces, so leaving redundant ones out is on you.
- **A declaration longer than 120 characters puts one parameter per line**, the closing parenthesis after the last one and the base list (`: Base<...>`) on its own indented line.
- Check with `dotnet format Archetypes.Api.slnx --verify-no-changes`.

## Comments

XML docs are not required here (`CS1591` is off): nothing outside this service consumes it. The exception is the `<param>` text of the request body records in `App.Api/Resources`: the OpenAPI generator reads it and the Scalar page shows it. A `//` comment says why, never what.

## While the model is in development

There is one migration, `InitialDb`, always recreated: `make db-reset` drops the database, removes every migration, adds `InitialDb` from the model and applies it ([database](../guides/database.md)). After a release, new changes are `make migration-add <Name>`.

## NuGet versions

Every dependency version is in `Directory.Packages.props` (central package management), the foundation packages included. A project writes `<PackageReference Include="Name" />` with no version; to add or update a dependency, change its `PackageVersion` there.

## The Bruno collection

Every endpoint added, changed or removed updates `bruno/`: the request in the folder of its resource, its assertions and `seq`, and the collection's README when a folder changes ([the collection](../../bruno/README.md)).
