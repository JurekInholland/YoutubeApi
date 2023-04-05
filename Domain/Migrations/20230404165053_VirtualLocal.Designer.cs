﻿// <auto-generated />
using System;
using Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Domain.Migrations
{
    [DbContext(typeof(YoutubeAppContext))]
    [Migration("20230404165053_VirtualLocal")]
    partial class VirtualLocal
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("Models.ApplicationSettings", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("DownloadPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("MaxVideoDuration")
                        .HasColumnType("TEXT");

                    b.Property<string>("NamingFormat")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ApplicationSettings");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            DownloadPath = "data/videos",
                            MaxVideoDuration = new TimeSpan(0, 1, 0, 0, 0),
                            NamingFormat = "{id} - {title}s{ext}"
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

                    b.Property<int>("Size")
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

                    b.Property<float>("Abr")
                        .HasColumnType("REAL");

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChannelId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChannelUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<string>("DurationString")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FileSize")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "filesize_approx");

                    b.Property<int>("Fps")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<long>("LikeCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LocalVideoId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uploader")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UploaderId")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "uploader_id");

                    b.Property<float>("Vbr")
                        .HasColumnType("REAL");

                    b.Property<long>("ViewCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WebpageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LocalVideoId");

                    b.ToTable("YoutubeVideos");
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
                    b.HasOne("Models.DomainModels.LocalVideo", "LocalVideo")
                        .WithMany()
                        .HasForeignKey("LocalVideoId");

                    b.Navigation("LocalVideo");
                });

            modelBuilder.Entity("Models.DomainModels.YoutubeVideo", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
