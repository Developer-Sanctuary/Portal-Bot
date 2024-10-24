using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Portal.DB;
using Portal.Embeds;
using Portal.Helpers;

namespace Portal.CommandHandlers;

public class PortalCommandHandler(BotDbContext dbContext,
    Helper helper) : ModuleBase<SocketCommandContext>
{
    [Command("teleport")]
    [Alias("portal", "tp")]
    [Summary("Opens a portal from current rxChannel to provided rxChannel")]
    public async Task HandleTeleport(SocketGuildChannel rxChannel)
    {
        if (rxChannel.Id == Context.Channel.Id)
        {
            await Context.Message.ReplyAsync(
                "You can't open portal in the same rxChannel...!");
            return;
        }

        if (rxChannel.GetChannelType() == ChannelType.Category)
        {
            await Context.Channel.SendMessageAsync(
                "You can't teleport to a rxChannel category...!");
            return;
        }

        if (!helper.IsChannelExcluded(rxChannel, Context.Guild, dbContext))
        {
            var referenceMessage = Context.Message.ReferencedMessage;
            await(rxChannel as ISocketMessageChannel)!.SendMessageAsync(
                embed: new RxPortalEmbed(Context.Channel, 
                    Context.Guild, referenceMessage as SocketMessage).Build());
        }

        await Context.Channel.SendMessageAsync(embed: new TxPortalEmbed(rxChannel, Context.Guild).Build());
        await helper.LogToLogChannelAsync(Context.Guild, Context.Channel.Id, rxChannel.Id);
    }
}