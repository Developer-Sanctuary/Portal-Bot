using Discord;

namespace Portal.Common;

public interface IEmbedProvider
{
    Embed Build();
}