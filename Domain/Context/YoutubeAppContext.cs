using Microsoft.EntityFrameworkCore;
using Models;

namespace Domain.Context;

public class YoutubeAppContext : DbContext
{
    public DbSet<YoutubeVideo> YoutubeVideos { get; set; } = null!;

    public YoutubeAppContext(DbContextOptions<YoutubeAppContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (optionsBuilder.IsConfigured) return;
        optionsBuilder.UseSqlite("Data Source=YoutubeVideos.db");
    }
}
