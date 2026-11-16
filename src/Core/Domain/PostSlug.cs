using System;
using System.Text.RegularExpressions;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Foundation.Core.Extensions;

namespace Aurore.Archetypes.Api.Core.Domain;

public static partial class PostSlug
{
    public static string Create(string title, Guid id)
    {
        var words = NotLettersOrDigits().Replace(title.RemoveDiacritics().ToLowerInvariant(), "-").Trim('-');

        if (words.Length > BlogRules.SlugTitleMaxLength)
        {
            // Cut at the last whole word that fits; a single word longer than the limit is cut where the limit is.
            var cut = words.LastIndexOf('-', BlogRules.SlugTitleMaxLength);
            words = words[..(cut > 0 ? cut : BlogRules.SlugTitleMaxLength)];
        }

        return words.Length is 0 ? id.ToString("N") : $"{words}-{id:N}";
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NotLettersOrDigits();
}
