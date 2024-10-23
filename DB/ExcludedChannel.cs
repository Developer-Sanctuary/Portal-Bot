using Discord;

namespace Portal.DB;
public class ExcludedChannel
{
    public ulong Id { get; set; }
    public string ChannelName { get; set; } = string.Empty;
    public ulong ChannelId { get; set; }
    public ChannelType Type { get; set; }
}