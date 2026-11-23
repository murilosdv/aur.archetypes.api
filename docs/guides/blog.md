[Home](../../README.md)

# The blog: authors, posts and comments

The domain of this service. It is small on purpose and uses most of what the foundation offers.

## Model

```
Author 1 ─── * Post 1 ─── * Comment
   └────────────────────────────* (an author writes both)
```

| Entity | Base | Notes |
|---|---|---|
| `Author` | `Entity` | An account linked to the identity by `Subject` (the `sub` claim, unique). `Handle` is unique and never changes; it is a name like a forum user name, not a slug: letters and digits with single hyphens or underscores between them, in any case (`my-name`, `my_name`, `MyName`). Not soft deletable on purpose: a filtered parent with unfiltered children makes joins return nothing. |
| `Post` | `SoftDeletableEntity` | `Slug` (made by the API from the title and the id of the post when it is written, never sent by the caller; unique), `Title`, `Body`, `Status` (`Draft` or `Published`, stored as text), `PublishedAt`. |
| `Comment` | `SoftDeletableEntity` | Belongs to a post, written by an author. |
| `CommentReport` | `Entity` | A complaint about a comment: the `Reason` and who made it. An author reports a comment once (unique index on comment and reporter). |

Entities are in `src/Core/Domain`, configurations in `src/Infra/Contexts/Configurations`, the migration in `tools/Tools.Migrations/History`.

## Who can do what

| | Reader | Editor | Admin |
|---|---|---|---|
| Read posts and comments | yes | yes | yes, drafts of everyone |
| Create the own author profile | yes | yes | yes |
| Comment (needs a profile) | yes | yes | yes |
| Report a comment (needs a profile) | yes | yes | yes |
| Write, change and publish posts | no | yes (own) | yes (any) |
| Delete comments and posts | no | no | yes |

Roles come from the token; the scopes `api.read` and `api.write` (`AppScopes` in Core) come from the client. In the dev-sbx Keycloak (realm `aurorelabs`), as machine callers: `caller-alice` is a reader, `caller-bob` an editor and admin.
The API key client in `appsettings.Development.json` can only read.

## Endpoints (`/v1`)

| | Who | Does |
|---|---|---|
| `PUT /authors/me` | `api.write` | Creates the caller's profile or changes its display name. Idempotent by nature. |
| `GET /posts` | `api.read` | Numbered page. Filters `title` (any case), `status`, `author` (a handle); ranges `createdFrom/To`, `updatedFrom/To`; `orderBy` title, author, publishedAt, createdAt (newest first by default). Others' drafts never appear. |
| `GET /feed` | `api.read` | Published posts as an infinite scroll (`cursor`, `pageSize`), newest first. Filters `title`, `author`; `orderBy` title, author, createdAt (not publishedAt: a cursor cannot page by a value that can be missing). |
| `GET /posts/{slug}` | `api.read` | A draft is a 404 for anyone but its author and admins. |
| `POST /posts` | editor, admin | A draft. Send only `title` and `body`: the API builds the slug (`my-adventure-in-europe-2026-<id>`: the title without accents or symbols, cut at a word, then the id). Accepts `Idempotency-Key`. |
| `PATCH /posts/{id}` | editor, admin | Changes only the fields sent (title, body); the slug never changes. The author or an admin. |
| `POST /posts/{id}/publish` | editor, admin | The author or an admin, once (409 `post.already_published`). |
| `DELETE /posts/{id}` | admin | Soft delete. |
| `GET /posts/{id}/comments` | `api.read` | Infinite scroll, oldest first. |
| `POST /posts/{id}/comments` | `api.write` | Only on published posts (409 `post.not_published`). Accepts `Idempotency-Key`. |
| `DELETE /posts/{id}/comments/{commentId}` | admin | Soft delete; a comment of another post is a 404. |
| `POST /posts/{id}/comments/{commentId}/report` | `api.write` | Body `{ "reason": "..." }`. 202, and the moderators get an email with the post, the comment, its author and the reason. Once per author (409 `comment.already_reported`). Accepts `Idempotency-Key`. |

Every endpoint that changes data also needs `api.write`; the roles above are on top of it.

Publishing a post and reporting a comment both email the moderators (`Email:ModeratorAddresses`).

Without a profile, writing a post or comment, or reporting one, is a 409 `author.profile_required`: the profile is created explicitly, never as a side effect.

## Where the rules live

- **Endpoints** (`src/App.Api/Resources`) validate the body's shape (`RequestValidator`) and build the response. The caller comes from the token (`ToCaller()`), never the body or query.
- **Features** (`src/Features`) hold the rules: the author profile (a `FeatureValidator`), draft visibility, who may publish, status checks. Unique conflicts (handle) are answered by the unique violation decorator with the errors registered for the indexes in `AddAppDatabase`.

## Trying it

With the dev-sbx up (PostgreSQL, Keycloak, and this service as a container at `http://127.0.0.1:5000`; see [running it](./deployment.md)), open the [Bruno collection](../../bruno/README.md) and run the `Auth` folder, then `Blog`, in order. The tokens come from Keycloak and the ids from the answers. The environment `Local` points at the container (`http://127.0.0.1:5000`); change `host` for a service run from the IDE (apply the migration to a local database first, see [database](./database.md)). You can also sign in on the API documentation page.

The story: `PUT /authors/me`, `POST /posts`, `POST /posts/{id}/publish`, `POST /posts/{id}/comments`, `GET /posts/{id}/comments`.

## A service shared by two features: the email sender

`IEmailSender` (`Core/Clients/Email`) is the example of a service that more than one feature uses. `ReportComment` and `PublishPost` both tell the moderators something, and neither knows how an email travels: they build a message with the words in `Features/Shared/ModerationEmails`, and the sender delivers it.

| Piece | Where | What it does |
|---|---|---|
| `IEmailSender`, `EmailMessage` | `Core/Clients/Email` | The contract: one message, plain text, a one-line subject (line breaks become spaces, so a title cannot add headers). |
| `TrySendAsync` | `Core/Clients/Email` | For a feature whose work is done before the email: a mail server that is down is logged as a warning and the feature still succeeds. Cancellation is not swallowed. |
| `SmtpEmailSender` | `Infra/Clients/Email` | The implementation, over the SMTP server of `Email` in the settings (`Host`, `Port`, `EnableSsl`, `Username`, `Password`, `From`). |
| `EmailOptions` | `Core/Settings`, validated by `EmailOptionsValidator` | `ModeratorAddresses` is who receives the reports. Production refuses a local server and a connection that is not encrypted. |

In development the server is the Mailpit of the dev-sbx: it keeps every email and sends none. Read them at `http://127.0.0.1:8025`. The tests replace the sender with `RecordingEmailSender`, so they read what would have been sent and can make the server "go down".

An outbox (the email written in the same transaction as the report, sent by a background service) is what makes the notice reliable; here a failure is only logged, to keep the example small.
