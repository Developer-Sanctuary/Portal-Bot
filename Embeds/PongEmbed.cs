using System.Collections.Immutable;
using Discord;
using Portal.Common;

namespace Portal.Embeds;

public class PongEmbed(int ping) : IEmbedProvider
{
    public Embed Build() => new EmbedBuilder()
        .WithTitle("Pong!")
        .WithDescription($"Latency: `{ping} ms`")
        .WithColor(Color.Blue)
        .Build();
}