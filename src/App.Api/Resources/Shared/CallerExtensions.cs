using System;
using System.Security.Claims;
using Aurore.Archetypes.Api.Core.Domain;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;

namespace Aurore.Archetypes.Api.App.Api.Resources.Shared;

internal static class CallerExtensions
{
    public static Caller ToCaller(this ClaimsPrincipal user)
    {
        var subject = user.GetUserId()
            ?? throw new InvalidOperationException("The caller has no subject. The endpoint must require authorization.");

        return new Caller(subject, user.IsInRole("admin"));
    }
}
