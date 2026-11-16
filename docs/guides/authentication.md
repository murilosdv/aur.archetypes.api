[Home](../../README.md)

# Authentication

How this service identifies its callers and decides what they may do. The mechanisms (JWT bearer, API keys, roles and scopes, pinning schemes) are the foundation's; its [authentication guide](https://github.com/murilosdv/aur.foundation/blob/main/docs/aspnetcore/authentication.md) documents every setting. This page is about how they are used here.

## Two kinds of caller

| Caller | Sends | Identity (`Caller.Subject`) | Gets roles and scopes from |
|---|---|---|---|
| A person or another service with a **Keycloak token** | `Authorization: Bearer <token>` | the token's `sub` | the token (realm roles, the client roles of audience `api`, and the `scope` claim) |
| A **trusted client with an API key** | `X-Api-Key: <key>` | the client's name in `ApiKeys:Clients` | `ApiKeys:Clients:<name>:Roles` and `:Scopes` |

Both arrive as the same thing in the code: a `ClaimsPrincipal` with roles and scopes. The key client `local-tester` in `appsettings.json` has the `reader` role and the `api.read` scope, so it can read and nothing else. In Production keys come from the environment (`ApiKeys__Clients__<name>__Key`) and must have 32 characters or more.

## What is required, where

- **Every resource group requires an authenticated caller** (the foundation's default). New endpoints fail closed.
- **Scopes** say which *kind of access* the client was granted: `api.read` for reads, `api.write` for anything that changes data (`AppScopes` in `Core`). Each endpoint requires one with `RequireScopes`.
- **Roles** say what the *person or service* may do inside that access: `reader`, `editor`, `admin`. Both must pass: a write needs the `api.write` scope **and** a role that allows that write ([who can do what](./blog.md#who-can-do-what)).
- Role rules that depend on the data (only the author or an admin may publish a post) are decided in the **feature**, from the `Caller`, not in the endpoint.

## The `Caller`

`Core/Domain/Caller` is `(Subject, IsAdmin)`. The endpoint builds it with `user.ToCaller()` (`App.Api/Resources/Shared/CallerExtensions.cs`) from the validated principal and passes it to the feature. It is **never read from the body or the query**: a client cannot claim to be somebody else.

## Settings used

| Key | Here |
|---|---|
| `IdentityProvider:AuthorityUrl` | `http://127.0.0.1:8080/realms/aurorelabs` in development: the dev-sbx Keycloak. Production must be https and not local. |
| `IdentityProvider:Audience` | `api` |
| `IdentityProvider:Provider` | `Keycloak` (roles from `realm_access` and `resource_access.api`) |
| `IdentityProvider:Scopes` | `api.read`, `api.write`: offered by the documentation page |
| `ApiDocs:OAuthClientId` | `api-docs`: the public PKCE client the Scalar page signs in with |
| `ApiKeys` | the `local-tester` client |

## The dev-sbx identities

The dev-sbx Keycloak (realm `aurorelabs`) has what the tests and the Bruno collection use:

| Identity | Type | Roles | Use |
|---|---|---|---|
| `caller-alice`, `caller-carol`, `caller-dave` | client credentials (secret `local-dev-secret`) | `reader` | Machine callers; scopes `api.read` and `api.write`. |
| `caller-bob` | client credentials | `admin`, `editor` | Machine caller that can write posts and delete. |
| `aur-archetypes-api` | client credentials (secret `local-dev-secret`) | | This service as a caller of the Orders dependency; scope `orders.read`. |
| `alice` (reader), `bob` (admin, editor) | users, password = name | | Signing in through the Scalar page (authorization code with PKCE, client `api-docs`). |

There is no password grant. Get a token for a machine caller with the requests of the `Auth` folder of the [Bruno collection](../../bruno/README.md).

## Testing it

`Tests.Integration/App.Api/Authentication` checks that real Keycloak tokens and the API key are accepted and map to the right roles; the blog's endpoint tests use real tokens for every "who may call this" case ([testing](../instructions/testing.md)).
