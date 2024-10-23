using Microsoft.EntityFrameworkCore;

namespace Portal.DB;

public sealed class BotDbContext : DbContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<ExcludedChannel> ExcludedChannels { get; set; }
    public DbSet<KVStore> KvStores { get; set; }
}