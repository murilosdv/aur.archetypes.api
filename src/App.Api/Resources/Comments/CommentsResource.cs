using Aurore.Foundation.AspNetCore.MinimalApis;

namespace Aurore.Archetypes.Api.App.Api.Resources.Comments;

internal sealed class CommentsResource : ResourceGroup
{
    public override string Name => "posts/{postId:guid}/comments";
}
