using Aurore.Archetypes.Api.App.Api.Resources.Comments.Endpoints;
using Aurore.Archetypes.Api.Core.Constants;

namespace Aurore.Archetypes.Api.Tests.App.Api.Resources.Comments;

[Trait("Feature", "Report input")]
public sealed class ReportCommentBodyValidatorTests
{
    private readonly ReportCommentBodyValidator _validator = new();

    [Theory(DisplayName = "A reason is required, with a limit on its length")]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("Spam", true)]
    public void ReasonIsRequired(string reason, bool valid)
    {
        // Arrange
        var body = new ReportCommentBody(reason);

        // Act
        var result = _validator.Validate(body);

        // Assert
        Assert.Equal(valid, result.IsValid);
    }

    [Fact(DisplayName = "A reason up to its limit is accepted and one character more is rejected")]
    public void ReasonHasALimit()
    {
        // Arrange
        var atTheLimit = new ReportCommentBody(new string('x', BlogRules.ReportReasonMaxLength));
        var tooLong = new ReportCommentBody(new string('x', BlogRules.ReportReasonMaxLength + 1));

        // Act
        var limitResult = _validator.Validate(atTheLimit);
        var tooLongResult = _validator.Validate(tooLong);

        // Assert
        Assert.True(limitResult.IsValid);
        Assert.Contains(tooLongResult.Errors, x => x.PropertyName == nameof(ReportCommentBody.Reason));
    }
}
