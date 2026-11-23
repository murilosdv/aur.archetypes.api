# Bruno collection

Requests for the API, to open with [Bruno](https://www.usebruno.com) (*Open Collection*, choose this folder) or to run from a terminal with the CLI (`npx @usebruno/cli run --env Local`, from this folder).

Select the environment **Local**: `host` is the service as a container (`http://127.0.0.1:5000`), `keycloak` is the token endpoint of the dev-sbx Keycloak and `apiKey` the local API key of `appsettings.Development.json`. For the service run from the IDE, change `host`.

| Folder | What it has |
|---|---|
| `Auth` | `Login bob` (editor and admin) and `Login alice` (reader). Run these first: they keep the tokens in the variables `bob` and `alice`, used by the other folders. |
| `Blog` | One folder per resource, each one able to run on its own: `Authors` (the two profiles), `Posts` (draft, who can see it, publishing, lists, editing, deleting), `Comments` (makes its own post first; the report emails are read in the Mailpit of the dev-sbx, `http://127.0.0.1:8025`), `Feed`. Ids and the slug are kept from the answers, and each request asserts the status it should get. |
| `Playground` | One folder per topic: `Auth`, `Caching`, `Dependencies`, `Diagnostics`, `Health`, `Idempotency`, `Layers`, `Pagination`, `RateLimiting`, `Timeouts`. The description of each request says what to look at. |

Needs the dev-sbx up (PostgreSQL, Keycloak and Mailpit), the service running and its database created (`make db-reset`, see [database](../docs/guides/database.md)). The folders can be run again and again: the profiles are a PUT and every run writes a new post (the idempotency keys are random).
