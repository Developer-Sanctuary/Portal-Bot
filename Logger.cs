using Discord;
using Serilog;
using Serilog.Events;

namespace Portal;

public class CustomBotLogger
{
    public async Task BotLogger(LogMessage logMessage)
    {
        LogEventLevel logLevel = logMessage.Severity switch
        {
            LogSeverity.Critical => LogEventLevel.Fatal,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Info => LogEventLevel.Information,
            LogSeverity.Verbose => LogEventLevel.Verbose,
            LogSeverity.Debug => LogEventLevel.Debug,
            _ => LogEventLevel.Debug
        };
        Log.Write(logLevel, logMessage.Exception, 
            "{source}: {message}", logMessage.Source, logMessage.Message);
        await Task.CompletedTask;
    }
}