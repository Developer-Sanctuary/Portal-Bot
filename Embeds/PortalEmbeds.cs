using Discord;
using Discord.WebSocket;
using Portal.Common;

namespace Portal.Embeds;

public class TxPortalEmbed(SocketGuildChannel channel, SocketGuild guild) : IEmbedProvider
{
    public Embed Build() => new EmbedBuilder()
        .WithTitle("Portal Opened")
        .WithDescription(
            $"A Portal to <#{channel.Id}> has been opened!\n[Teleport in](https://discord.com/channels/{guild.Id}/{channel.Id})")
        .WithThumbnailUrl("https://i.gifer.com/origin/c7/c795fccbcc24322a9107cca44252227b_w200.gif")
        .WithColor(0xD9AE9A)
        .Build();
}

public class RxPortalEmbed(SocketGuildChannel channel, SocketGuild guild, SocketMessage? annotatedMessage = null) : IEmbedProvider
{
    public Embed Build()
    {
        var embed = new EmbedBuilder()
            .WithTitle("Portal Opened")
            .WithDescription(
                $"A Portal has been opened from <#{channel.Id}>!\n[Teleport back](https://discord.com/channels/{guild.Id}/{channel.Id})")
            .WithThumbnailUrl("https://i.gifer.com/origin/c7/c795fccbcc24322a9107cca44252227b_w200.gif")
            .WithColor(0x9392BA);
        if (annotatedMessage != null || !string.IsNullOrEmpty(annotatedMessage!.Content))
        {
            embed.AddField("Annotated Message", 
                $"{annotatedMessage.Content} [Go to Message](https://discord.com/channels/{guild.Id}/{channel.Id}/{annotatedMessage.Id})");
        }
        return embed.Build();
    }
}