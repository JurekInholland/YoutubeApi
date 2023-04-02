using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Models;

namespace Domain.Context;

public class YoutubeAppContext : DbContext
{
    public DbSet<YoutubeVideo> YoutubeVideos { get; set; } = null!;
    public DbSet<QueuedDownload> QueuedDownloads { get; set; } = null!;

    public YoutubeAppContext(DbContextOptions<YoutubeAppContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RelatedVideo>().Property(e => e.Tags)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Metadata.SetValueComparer(new ValueComparer<string[]>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToArray()));


        modelBuilder.Entity<YoutubeVideo>().Property(e => e.Categories)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Metadata.SetValueComparer(new ValueComparer<string[]>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToArray()));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (optionsBuilder.IsConfigured) return;
        optionsBuilder.UseSqlite("Data Source=youtube.db");
    }
}
