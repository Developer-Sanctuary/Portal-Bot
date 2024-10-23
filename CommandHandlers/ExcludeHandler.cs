using Discord;
using Discord.Interactions;
using Portal.DB;
using Microsoft.EntityFrameworkCore;

namespace Portal.CommandHandlers;

[RequireUserPermission(GuildPermission.Administrator)]
public class ExcludeHandler(BotDbContext dbContext): InteractionModuleBase<SocketInteractionContext>
{
    // /exclude - Invoke this command to set channels that are read only
    [SlashCommand("exclude", "Exclude a channel for being read only")]
    public async Task HandleExclude()
    {
        var channels = Context.Guild.Channels;
        var menuBuilder = new SelectMenuBuilder()
            .WithCustomId("exclude_channel")
            .WithPlaceholder("Select a channel to exclude")
            .WithMaxValues(0)
            .WithMaxValues(channels.Count > 25 ? 25 : channels.Count);

        List<SelectMenuOptionBuilder> options = new();
        // Assigning #️⃣ emoji for individual channels and :folder_icon: emoji fot Category channels
        foreach (var channel in channels)
        {
            var isSelected = dbContext.ExcludedChannels.Any(x => x.ChannelId == channel.Id);
            if (channel.GetChannelType() == ChannelType.Category)
            {
                options.Add(new SelectMenuOptionBuilder(
                    channel.Name, channel.Id.ToString(), 
                    emote: Emote.Parse("<:folder_icon:1297977331230707744>"),
                    isDefault: isSelected));
            }
            else
            {
                options.Add(new SelectMenuOptionBuilder(
                    channel.Name, channel.Id.ToString(), emote: new Emoji("#️⃣"),
                    isDefault: isSelected));
            }
        }

        menuBuilder.WithOptions(options);

        await RespondAsync("Select the channels you want to exclude from opening portal.",
            components: new ComponentBuilder().WithSelectMenu(menuBuilder).Build(), ephemeral: true);
    }

    // Gets invoked when a selection of channels to exclude is done
    [ComponentInteraction("exclude_channel", ignoreGroupNames: true)]
    public async Task HandleChannelSelectCommand(string[] channels)
    {
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM ExcludedChannels");
        
        // Iterating through each selection and adding to DB
        foreach (var channelId in channels)
        {
            var channel = Context.Guild.GetChannel(Convert.ToUInt64(channelId));
            var channelType = channel.GetChannelType();
            if (channelType is { } cType)
            {
                dbContext.ExcludedChannels.Add(new ExcludedChannel()
                {
                    ChannelId = channel.Id,
                    ChannelName = channel.Name,
                    Type = cType
                });
            }
        }

        await dbContext.SaveChangesAsync();
        await RespondAsync("Settings applied successfully!", ephemeral: true);
    }
}