using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Portal.DB;

namespace Portal.CommandHandlers;

[RequireUserPermission(GuildPermission.Administrator)]
public class LogChannelHandler(BotDbContext dbContext) : InteractionModuleBase<SocketInteractionContext>
{
    // /logchannel - Assigns a specific text channel for portal open logs
    [SlashCommand("logchannel", "Sets the channel where the bot logs information")]
    public async Task HandleLogChannel(SocketGuildChannel channel)
    {
        if (channel.GetChannelType() != ChannelType.Text)
        {
            await RespondAsync("This type of channel can't be used for logging...!", ephemeral: true);
        }
        
        // Adding db entry with Key as 'LogChannelId' and value as the Channel ID
        dbContext.KvStores.Add(new KVStore()
        {
            Key = "LogChannelId",
            Value = channel.Id.ToString(),
        });
        await dbContext.SaveChangesAsync();
        await RespondAsync("Selection saved successfully!", ephemeral: true);
    }
}