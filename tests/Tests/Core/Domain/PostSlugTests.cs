using System;
using System.Linq;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Core.Domain;

namespace Aurore.Archetypes.Api.Tests.Core.Domain;

[Trait("Feature", "Post slug")]
public sealed class PostSlugTests
{
    private static readonly Guid _id = Guid.Parse("0192f3a4-7b1c-7d2e-8f3a-4b5c6d7e8f90");
    private const string IdText = "0192f3a47b1c7d2e8f3a4b5c6d7e8f90";

    public static TheoryData<string, string> Titles => new()
    {
        { "My adventure through Europe in 2026", "my-adventure-through-europe-in-2026" },
        { "Ação, coração & café!", "acao-coracao-cafe" },
        { "  --Hello___World--  ", "hello-world" },
        { "Already-kebab-case", "already-kebab-case" },
        { "MiXeD CaSe 42", "mixed-case-42" },
    };

    [Theory(DisplayName = "The slug is the title in lower case, without accents or symbols, words joined by hyphens, then the id")]
    [MemberData(nameof(Titles))]
    public void SlugIsTheTitleThenTheId(string title, string words)
    {
        // Arrange
        // Act
        var slug = PostSlug.Create(title, _id);

        // Assert
        Assert.Equal($"{words}-{IdText}", slug);
    }

    [Fact(DisplayName = "A long title is cut at a whole word, and the slug always fits its column")]
    public void LongTitleIsCutAtAWord()
    {
        // Arrange
        var title = string.Join(' ', Enumerable.Repeat("adventure", 30));

        // Act
        var slug = PostSlug.Create(title, _id);

        // Assert
        Assert.True(slug.Length <= BlogRules.SlugMaxLength);
        Assert.StartsWith("adventure-adventure-", slug);
        Assert.EndsWith($"adventure-{IdText}", slug);
        Assert.True(slug.Length - IdText.Length - 1 <= BlogRules.SlugTitleMaxLength);
    }

    [Fact(DisplayName = "A single word longer than the limit is cut where the limit is")]
    public void LongWordIsCut()
    {
        // Arrange
        var title = new string('a', BlogRules.SlugTitleMaxLength + 20);

        // Act
        var slug = PostSlug.Create(title, _id);

        // Assert
        Assert.Equal($"{new string('a', BlogRules.SlugTitleMaxLength)}-{IdText}", slug);
    }

    [Theory(DisplayName = "A title with no letter or digit left gives the id alone")]
    [InlineData("!!!")]
    [InlineData("日本語")]
    [InlineData("   ")]
    public void TitleWithNothingLeftGivesTheId(string title)
    {
        // Arrange
        // Act
        var slug = PostSlug.Create(title, _id);

        // Assert
        Assert.Equal(IdText, slug);
    }
}
