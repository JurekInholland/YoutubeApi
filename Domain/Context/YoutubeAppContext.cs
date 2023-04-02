using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Domain.Context;

public class YoutubeAppContext : DbContext
{
    public DbSet<YoutubeVideo> YoutubeVideos { get; set; } = null!;

    public YoutubeAppContext(DbContextOptions<YoutubeAppContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RelatedVideo>().Property(e => e.Tags)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (optionsBuilder.IsConfigured) return;
        optionsBuilder.UseSqlite("Data Source=YoutubeVideos.db");
    }
}
