using Aurore.Archetypes.Api.App.Api.Resources.Authors.Endpoints;
using Aurore.Archetypes.Api.Core.Constants;

namespace Aurore.Archetypes.Api.Tests.App.Api.Resources.Authors;

[Trait("Feature", "Author profile input")]
public sealed class PutAuthorBodyValidatorTests
{
    private readonly PutAuthorBodyValidator _validator = new();

    public static TheoryData<string> ValidHandles => ["abc", "alice", "alice-b", "alice_b", "AliceB", "MyName", "a1-b2_c3", "007", new string('a', BlogRules.HandleMaxLength)];

    public static TheoryData<string> InvalidHandles =>
    [
        string.Empty,
        "ab",
        new string('a', BlogRules.HandleMaxLength + 1),
        "-alice",
        "alice-",
        "_alice",
        "alice_",
        "alice--b",
        "alice__b",
        "alice-_b",
        "alice.b",
        "ação",
        "has space",
    ];

    [Theory(DisplayName = "A handle of 3 to 32 letters and digits in any case, with single hyphens or underscores between them (kebab, snake or Pascal), is accepted")]
    [MemberData(nameof(ValidHandles))]
    public void ValidHandleIsAccepted(string handle)
    {
        // Arrange
        var body = new PutAuthorBody(handle, "Alice");

        // Act
        var result = _validator.Validate(body);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory(DisplayName = "A handle that is empty, too short or long, or not made of letters and digits with single hyphens or underscores between them is rejected")]
    [MemberData(nameof(InvalidHandles))]
    public void InvalidHandleIsRejected(string handle)
    {
        // Arrange
        var body = new PutAuthorBody(handle, "Alice");

        // Act
        var result = _validator.Validate(body);

        // Assert
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(PutAuthorBody.Handle));
    }

    [Fact(DisplayName = "The display name is required and has a limit on its length")]
    public void DisplayNameIsRequiredAndLimited()
    {
        // Arrange
        var empty = new PutAuthorBody("alice", string.Empty);
        var tooLong = new PutAuthorBody("alice", new string('x', BlogRules.DisplayNameMaxLength + 1));
        var atTheLimit = new PutAuthorBody("alice", new string('x', BlogRules.DisplayNameMaxLength));

        // Act
        var emptyResult = _validator.Validate(empty);
        var tooLongResult = _validator.Validate(tooLong);
        var limitResult = _validator.Validate(atTheLimit);

        // Assert
        Assert.Contains(emptyResult.Errors, x => x.PropertyName == nameof(PutAuthorBody.DisplayName));
        Assert.Contains(tooLongResult.Errors, x => x.PropertyName == nameof(PutAuthorBody.DisplayName));
        Assert.True(limitResult.IsValid);
    }
}
