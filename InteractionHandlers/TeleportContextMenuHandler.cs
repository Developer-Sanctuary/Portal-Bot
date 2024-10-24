using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Portal.DB;
using Portal.Embeds;
using Portal.Helpers;

namespace Portal.InteractionHandlers;

public class TeleportContextMenuHandler(
    BotDbContext dbContext, Helper helper) : InteractionModuleBase<SocketInteractionContext>
{
    // Handler for the context menu button "Open Portal" returns a select menu component with
    // channels as selection
    [MessageCommand("Open Portal")]
    public async Task HandleTeleport(SocketMessage message)
    {
        var selectMenu = new SelectMenuBuilder()
            .WithCustomId($"teleport_context_{message.Id}")
            .WithChannelTypes(ChannelType.Text, ChannelType.Voice, ChannelType.Forum)
            .WithType(ComponentType.ChannelSelect);
        await RespondAsync("Which channel you want to teleport to?",
            components: new ComponentBuilder().WithSelectMenu(selectMenu).Build(),
            ephemeral: true);
    }

    // Handles the response from select menu component which was later created with
    // "Open Portal" context command
    // Same as TeleportHandler.HandleTeleport()
    [ComponentInteraction("teleport_context_*", ignoreGroupNames: true)]
    public async Task HandleContextTeleport(string messageId, string[] channelIds)
    {
        var message = await Context.Channel.GetMessageAsync(Convert.ToUInt64(messageId));
        var rxChannel = Context.Guild.GetChannel(Convert.ToUInt64(channelIds[0]));
        if (rxChannel.Id == Context.Channel.Id)
        {
            await RespondAsync("You can't open portal in the same channel...!", ephemeral: true);
            return;
        }
        
        if (!helper.IsChannelExcluded(rxChannel, Context.Guild, dbContext))
        {
            await (rxChannel as ISocketMessageChannel)!.SendMessageAsync(
                embed: new RxPortalEmbed((Context.Channel as SocketGuildChannel)!, Context.Guild, message as SocketMessage).Build());
        }
        await RespondAsync(embed: new TxPortalEmbed(rxChannel, Context.Guild).Build());
        await helper.LogToLogChannelAsync(Context.Guild, Context.Channel.Id, rxChannel.Id);
    }
}