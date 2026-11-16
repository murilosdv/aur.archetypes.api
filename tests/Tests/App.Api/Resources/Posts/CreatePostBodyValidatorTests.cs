using System.Linq;
using Aurore.Archetypes.Api.App.Api.Resources.Posts.Endpoints;
using Aurore.Archetypes.Api.Core.Constants;

namespace Aurore.Archetypes.Api.Tests.App.Api.Resources.Posts;

[Trait("Feature", "Post input")]
public sealed class CreatePostBodyValidatorTests
{
    private readonly CreatePostBodyValidator _validator = new();

    [Fact(DisplayName = "The title and the body are required, with a limit on their length")]
    public void TitleAndBodyAreRequiredAndLimited()
    {
        // Arrange
        var empty = new CreatePostBody(string.Empty, string.Empty);
        var tooLong = new CreatePostBody(new string('x', BlogRules.TitleMaxLength + 1), new string('x', BlogRules.PostBodyMaxLength + 1));
        var atTheLimits = new CreatePostBody(new string('x', BlogRules.TitleMaxLength), new string('x', BlogRules.PostBodyMaxLength));

        // Act
        var emptyResult = _validator.Validate(empty);
        var tooLongResult = _validator.Validate(tooLong);
        var limitsResult = _validator.Validate(atTheLimits);

        // Assert
        Assert.Equal([nameof(CreatePostBody.Title), nameof(CreatePostBody.Body)], emptyResult.Errors.Select(x => x.PropertyName).Distinct());
        Assert.Equal([nameof(CreatePostBody.Title), nameof(CreatePostBody.Body)], tooLongResult.Errors.Select(x => x.PropertyName).Distinct());
        Assert.True(limitsResult.IsValid);
    }
}
