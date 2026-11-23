[Home](./README.md)

# Changelog

All notable changes to this project will be documented in this file.

Read [the instructions file](./docs/instructions/changelog.md) to learn what and how to write here.

## [Unreleased]

### Added

- A blog (authors, posts and comments) on PostgreSQL, as the example domain: the author profile, drafts and publishing, numbered pages of posts, an infinite scroll feed and comments, soft delete, and handle conflicts answered from the database's unique indexes. Writers are editors and admins by role; every write also needs the `api.write` scope.
- Sign-in with Keycloak tokens (JWT bearer, roles and scopes) or a read-only API key, with the OpenAPI document and a Scalar page that signs in by PKCE.
- Typed application settings, bound strictly and validated at startup, with Staging and Production files that take their values from the environment.
- A playground of the platform's features (authentication, caching, dependencies, idempotency, pagination, rate limiting, results and errors, timeouts) that can be deleted by following its guide.
- `Tools.Migrations` for the Entity Framework tools, with the initial migration, and `make` targets for migrations and the database (`make db-reset` recreates it while the model is in development).
- Reporting a comment (`POST /v1/posts/{id}/comments/{commentId}/report`), with a shared email service (`IEmailSender`) that also tells the moderators when a post is published; the dev-sbx has a Mailpit to read the emails.
- A [Bruno](https://www.usebruno.com) collection (`bruno`) in the OpenCollection YAML format with the sign-in requests, the blog story and the playground examples.
- A Dockerfile (non-root, port 8080), a compose file for the dev-sbx and a containerized test run with a coverage report. The foundation comes from nuget.org, so the image build needs no credentials.
- Unit tests of the settings files and input rules, and integration tests against a real PostgreSQL and real Keycloak tokens.
- Code conventions (files and folders, names, style, comments), and a `Shared` folder for what endpoints or features have in common.
- Guides: architecture, the blog, authentication, adding a feature, settings, the database, running it, the playground, using the project as a template, and testing.
- `Directory.Packages.props`: every NuGet version in one file (central package management), the foundation packages included (`1.0.0-preview.4`, from nuget.org).

---
