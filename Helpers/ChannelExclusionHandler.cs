using Discord;
using Discord.WebSocket;
using Portal.DB;
using Microsoft.EntityFrameworkCore;

namespace Portal.Helpers;

public class Helper(BotDbContext dbContext)
{
    public bool IsChannelExcluded(SocketGuildChannel channel, SocketGuild guild, BotDbContext dbContext)
    {
        var excludedChannels = dbContext.ExcludedChannels.AsNoTracking();
        foreach (var excludedChannel in excludedChannels)
        {
            if (excludedChannel.Type == ChannelType.Category)
            {
                var channelObj = guild.GetChannel(excludedChannel.ChannelId) as SocketCategoryChannel;
                if (channelObj!.Channels.Any(categoryChannel => categoryChannel.Id == channel.Id))
                {
                    return true;
                }
            }
            else
            {
                return excludedChannels.Any(x => x.ChannelId == channel.Id);
            }
        }
        return false;
    }

    public async Task LogToLogChannelAsync(SocketGuild guild, ulong txChannelId, ulong rxChannelId)
    {
        var logChannelId = dbContext.KvStores.FirstOrDefault(x => x.Key == "LogChannelId");
        if (logChannelId == null) return;
        var logChannel = guild.GetChannel(Convert.ToUInt64(logChannelId.Value));
        await (logChannel as ISocketMessageChannel)!.SendMessageAsync(
            $"Portal opened from <#{txChannelId}> -> <#{rxChannelId}>");
    }
}