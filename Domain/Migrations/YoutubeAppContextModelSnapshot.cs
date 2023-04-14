﻿// <auto-generated />
using System;
using Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Domain.Migrations
{
    [DbContext(typeof(YoutubeAppContext))]
    partial class YoutubeAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("Models.ApplicationSettings", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<long>("BackupIntervalSeconds")
                        .HasColumnType("INTEGER");

                    b.Property<long>("CheckForNewVideosIntervalSeconds")
                        .HasColumnType("INTEGER");

                    b.Property<long>("CleanUpInterval")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DownloadPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("MaxVideoDuration")
                        .HasColumnType("TEXT");

                    b.Property<string>("NamingFormat")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("UpdateChannelsIntervalSeconds")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UpdateVideosIntervalSeconds")
                        .HasColumnType("INTEGER");

                    b.Property<long>("WorkInterval")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ApplicationSettings");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            BackupIntervalSeconds = 0L,
                            CheckForNewVideosIntervalSeconds = 0L,
                            CleanUpInterval = 5000L,
                            DownloadPath = "data/videos",
                            MaxVideoDuration = new TimeSpan(0, 1, 0, 0, 0),
                            NamingFormat = "{id} - {title}s{ext}",
                            UpdateChannelsIntervalSeconds = 0L,
                            UpdateVideosIntervalSeconds = 0L,
                            WorkInterval = 1000L
                        });
                });

            modelBuilder.Entity("Models.DomainModels.LocalVideo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<float>("Abr")
                        .HasColumnType("REAL");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Fps")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Vbr")
                        .HasColumnType("REAL");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("LocalVideos");
                });

            modelBuilder.Entity("Models.DomainModels.QueuedDownload", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("QueuedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VideoId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("QueuedDownloads");
                });

            modelBuilder.Entity("Models.DomainModels.YoutubeChannel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Subscribers")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ThumbnailUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("YoutubeChannels");
                });

            modelBuilder.Entity("Models.DomainModels.YoutubeComment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "author_id");

                    b.Property<bool>("AuthorIsUploader")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AuthorThumbnail")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "author_thumbnail");

                    b.Property<bool>("IsFavorited")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LikeCount")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "like_count");

                    b.Property<string>("ParentId")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "parent_id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TimeText")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "time_text");

                    b.Property<string>("YoutubeVideoId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("YoutubeVideoId");

                    b.ToTable("YoutubeComment");
                });

            modelBuilder.Entity("Models.DomainModels.YoutubeVideo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<long>("LikeCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RelatedVideos")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("TEXT");

                    b.Property<long>("ViewCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WebpageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.Property<string>("YoutubeChannelId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("playableInEmbed")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("YoutubeChannelId");

                    b.ToTable("YoutubeVideos");
                });

            modelBuilder.Entity("Models.DomainModels.LocalVideo", b =>
                {
                    b.HasOne("Models.DomainModels.YoutubeVideo", null)
                        .WithOne("LocalVideo")
                        .HasForeignKey("Models.DomainModels.LocalVideo", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.DomainModels.QueuedDownload", b =>
                {
                    b.HasOne("Models.DomainModels.YoutubeVideo", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Models.DomainModels.YoutubeComment", b =>
                {
                    b.HasOne("Models.DomainModels.YoutubeVideo", null)
                        .WithMany("Comments")
                        .HasForeignKey("YoutubeVideoId");
                });

            modelBuilder.Entity("Models.DomainModels.YoutubeVideo", b =>
                {
                    b.HasOne("Models.DomainModels.YoutubeChannel", "YoutubeChannel")
                        .WithMany("Videos")
                        .HasForeignKey("YoutubeChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("YoutubeChannel");
                });

            modelBuilder.Entity("Models.DomainModels.YoutubeChannel", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("Models.DomainModels.YoutubeVideo", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("LocalVideo");
                });
#pragma warning restore 612, 618
        }
    }
}
