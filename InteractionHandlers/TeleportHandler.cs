using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Portal.DB;
using Portal.Embeds;
using Portal.Helpers;

namespace Portal.InteractionHandlers;

public class TeleportHandler(BotDbContext dbContext, Helper helper) : InteractionModuleBase<SocketInteractionContext>
{
    // /teleport - this command does the main teleportation things and opening
    // and closing of portals
    [SlashCommand("teleport", "Opens a portal from current rxChannel to provided rxChannel")]
    public async Task HandleTeleport([Name("channel")]SocketGuildChannel rxChannel)
    {
        if (rxChannel.Id == Context.Channel.Id)
        {
            await RespondAsync("You can't open portal in the same rxChannel...!", ephemeral: true);
            return;
        }

        if (rxChannel.GetChannelType() == ChannelType.Category)
        {
            await RespondAsync("You can't teleport to a rxChannel category...!", ephemeral: true);
            return;
        }

        if (!helper.IsChannelExcluded(rxChannel, Context.Guild, dbContext))
        {
            await(rxChannel as ISocketMessageChannel)!.SendMessageAsync(
                embed: new RxPortalEmbed((Context.Channel as SocketGuildChannel)!, Context.Guild).Build());
        }

        await RespondAsync(embed: new TxPortalEmbed(rxChannel, Context.Guild).Build());
        await helper.LogToLogChannelAsync(Context.Guild, Context.Channel.Id, rxChannel.Id);
    }
}