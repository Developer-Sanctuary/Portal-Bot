using Discord;
using Portal.Common;
// ReSharper disable All

namespace Portal.Embeds;

public class HelpEmbed : IEmbedProvider
{
    public Embed Build()
    {
        const string description = """
                                   Made with ❤️ by [CosmicPredator](https://github.com/CosmicPredator)
                                   ## Commands Reference

                                   `/teleport {channel}`
                                   Opens a portal from current channel to provided rxChannel.
                                   Alias: `!tp`, `!teleport`, `!portal`

                                   `/exclude`
                                   Exclude a channel for being read only.

                                   `/logchannel {channel}`
                                   Sets the channel where the bot logs information.

                                   """;
        var embed = new EmbedBuilder()
            .WithAuthor("Portal bot v1.0")
            .WithDescription(description)
            .WithColor(Color.Green)
            .WithFooter("# Glory to CPP");
        return embed.Build();
    }
}