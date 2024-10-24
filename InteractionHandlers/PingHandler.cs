using Discord;
using Discord.Interactions;
using Portal.Embeds;

namespace Portal.InteractionHandlers;

[RequireUserPermission(GuildPermission.Administrator)]
public class PingHandler : InteractionModuleBase<SocketInteractionContext>
{
    // /ping - dummy command to handle ping and pong and gives us the client latency
    [SlashCommand("ping", "Pings the system latency")]
    public async Task HandlePingAsync()
    {
        await RespondAsync(embed: new PongEmbed(Context.Client.Latency).Build(), ephemeral: true);
    }
}