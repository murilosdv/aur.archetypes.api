using Aurore.Archetypes.Api.App.Api.Resources.Comments.Endpoints;
using Aurore.Archetypes.Api.Core.Constants;

namespace Aurore.Archetypes.Api.Tests.App.Api.Resources.Comments;

[Trait("Feature", "Comment input")]
public sealed class CreateCommentBodyValidatorTests
{
    private readonly CreateCommentBodyValidator _validator = new();

    [Fact(DisplayName = "A comment of up to its limit is accepted")]
    public void CommentWithinTheLimitIsAccepted()
    {
        // Arrange
        var body = new CreateCommentBody(new string('x', BlogRules.CommentBodyMaxLength));

        // Act
        var result = _validator.Validate(body);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "An empty comment, or one over its limit, is rejected")]
    public void EmptyOrTooLongCommentIsRejected()
    {
        // Arrange
        var empty = new CreateCommentBody(string.Empty);
        var tooLong = new CreateCommentBody(new string('x', BlogRules.CommentBodyMaxLength + 1));

        // Act
        var emptyResult = _validator.Validate(empty);
        var tooLongResult = _validator.Validate(tooLong);

        // Assert
        Assert.False(emptyResult.IsValid);
        Assert.False(tooLongResult.IsValid);
    }
}
