using Aurore.Archetypes.Api.App.Api.Resources.Posts.Endpoints;
using Aurore.Archetypes.Api.Core.Constants;

namespace Aurore.Archetypes.Api.Tests.App.Api.Resources.Posts;

[Trait("Feature", "Post input")]
public sealed class PatchPostBodyValidatorTests
{
    private readonly PatchPostBodyValidator _validator = new();

    public static TheoryData<bool, bool> Accepted => new()
    {
        { true, false },
        { false, true },
        { true, true },
    };

    [Theory(DisplayName = "Sending the title, the body or both is accepted")]
    [MemberData(nameof(Accepted))]
    public void AtLeastOneFieldIsAccepted(bool sendTitle, bool sendBody)
    {
        // Arrange
        var body = new PatchPostBody(sendTitle ? "A title" : null, sendBody ? "A text" : null);

        // Act
        var result = _validator.Validate(body);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "Sending neither the title nor the body is rejected")]
    public void NothingToChangeIsRejected()
    {
        // Arrange
        var body = new PatchPostBody(null, null);

        // Act
        var result = _validator.Validate(body);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "A field that is sent cannot be empty or over its limit")]
    public void SentFieldsAreChecked()
    {
        // Arrange
        var emptyTitle = new PatchPostBody(string.Empty, "A text");
        var longBody = new PatchPostBody(null, new string('x', BlogRules.PostBodyMaxLength + 1));
        var longTitle = new PatchPostBody(new string('x', BlogRules.TitleMaxLength + 1), null);

        // Act
        var emptyTitleResult = _validator.Validate(emptyTitle);
        var longBodyResult = _validator.Validate(longBody);
        var longTitleResult = _validator.Validate(longTitle);

        // Assert
        Assert.Contains(emptyTitleResult.Errors, x => x.PropertyName == nameof(PatchPostBody.Title));
        Assert.Contains(longBodyResult.Errors, x => x.PropertyName == nameof(PatchPostBody.Body));
        Assert.Contains(longTitleResult.Errors, x => x.PropertyName == nameof(PatchPostBody.Title));
    }
}
