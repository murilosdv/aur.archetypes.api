using System;
using System.Linq;
using System.Security.Claims;
using Aurore.Archetypes.Api.App.Api.Resources.Shared;

namespace Aurore.Archetypes.Api.Tests.App.Api.Resources;

[Trait("Feature", "Caller")]
public sealed class CallerExtensionsTests
{
    [Fact(DisplayName = "The caller is the subject of the token, and an admin when it has the admin role")]
    public void CallerComesFromTheClaims()
    {
        // Arrange
        var admin = Principal((ClaimTypes.NameIdentifier, "sub-1"), (ClaimTypes.Role, "admin"));
        var reader = Principal((ClaimTypes.NameIdentifier, "sub-2"), (ClaimTypes.Role, "reader"));

        // Act
        var adminCaller = admin.ToCaller();
        var readerCaller = reader.ToCaller();

        // Assert
        Assert.Equal("sub-1", adminCaller.Subject);
        Assert.True(adminCaller.IsAdmin);
        Assert.Equal("sub-2", readerCaller.Subject);
        Assert.False(readerCaller.IsAdmin);
    }

    [Fact(DisplayName = "A caller with no subject is a mistake of the endpoint and throws")]
    public void CallerWithoutSubjectThrows()
    {
        // Arrange
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        // Act
        var act = anonymous.ToCaller;

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    private static ClaimsPrincipal Principal(params (string Type, string Value)[] claims)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(claims.Select(x => new Claim(x.Type, x.Value)), "test"));
    }
}
