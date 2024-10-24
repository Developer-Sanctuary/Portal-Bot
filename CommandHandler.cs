using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;

namespace Portal;

public class CommandHandler(DiscordSocketClient discordSocketClient,
    CommandService commandService, IServiceProvider serviceProvider)
{
    public async Task InstallCommandsAsync()
    {
        await commandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), serviceProvider);
        discordSocketClient.MessageReceived += HandleCommandAsync;
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        var message = messageParam as SocketUserMessage;
        if (message == null) return;
        
        int argPos = 0;

        if (!message.HasCharPrefix('!', ref argPos) ||
            message.HasMentionPrefix(discordSocketClient.CurrentUser, ref argPos) ||
            message.Author.IsBot)
        {
            return;
        }

        var context = new SocketCommandContext(discordSocketClient, message);
        await commandService.ExecuteAsync(
            context: context,
            argPos: argPos,
            services: serviceProvider);
    }
}