using Aurore.Foundation.Core.Constants;

namespace Aurore.Archetypes.Api.Core.Constants;

public static class BlogRules
{
    public const int SubjectMaxLength = FieldLengths.XLarge;
    public const int HandleMaxLength = FieldLengths.Small;
    public const int HandleMinLength = 3;
    public const int DisplayNameMaxLength = FieldLengths.Medium;
    public const int SlugMaxLength = FieldLengths.Large;
    public const int SlugTitleMaxLength = 60;
    public const int TitleMaxLength = FieldLengths.Large;
    public const int PostBodyMaxLength = FieldLengths.LongText;
    public const int CommentBodyMaxLength = FieldLengths.Text;
    public const int ReportReasonMaxLength = FieldLengths.XLarge;
    public const int StatusMaxLength = FieldLengths.Tiny;
}
