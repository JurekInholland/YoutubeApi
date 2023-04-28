using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    NamingFormat = table.Column<string>(type: "TEXT", nullable: false),
                    DownloadPath = table.Column<string>(type: "TEXT", nullable: false),
                    MaxVideoDuration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CleanUpInterval = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdateChannelsIntervalSeconds = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdateVideosIntervalSeconds = table.Column<long>(type: "INTEGER", nullable: false),
                    CheckForNewVideosIntervalSeconds = table.Column<long>(type: "INTEGER", nullable: false),
                    BackupIntervalSeconds = table.Column<long>(type: "INTEGER", nullable: false),
                    WorkInterval = table.Column<long>(type: "INTEGER", nullable: false),
                    CurrentTask = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeChannels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Handle = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", nullable: false),
                    BannerUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Subscribers = table.Column<string>(type: "TEXT", nullable: false),
                    VideoCount = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscribedChannels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<string>(type: "TEXT", nullable: true),
                    SubscriptionType = table.Column<int>(type: "INTEGER", nullable: false),
                    LastChecked = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribedChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscribedChannels_YoutubeChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "YoutubeChannels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YoutubeVideos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Thumbnail = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    WebpageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ViewCount = table.Column<long>(type: "INTEGER", nullable: false),
                    LikeCount = table.Column<long>(type: "INTEGER", nullable: false),
                    Categories = table.Column<string>(type: "TEXT", nullable: false),
                    RelatedVideos = table.Column<string>(type: "TEXT", nullable: false),
                    playableInEmbed = table.Column<bool>(type: "INTEGER", nullable: false),
                    YoutubeChannelId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeVideos_YoutubeChannels_YoutubeChannelId",
                        column: x => x.YoutubeChannelId,
                        principalTable: "YoutubeChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalVideos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    Extension = table.Column<string>(type: "TEXT", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Fps = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: false),
                    Vbr = table.Column<float>(type: "REAL", nullable: false),
                    Abr = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalVideos_YoutubeVideos_Id",
                        column: x => x.Id,
                        principalTable: "YoutubeVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueuedDownloads",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    QueuedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VideoId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueuedDownloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueuedDownloads_YoutubeVideos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "YoutubeVideos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YoutubeComment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: false),
                    AuthorThumbnail = table.Column<string>(type: "TEXT", nullable: false),
                    AuthorIsUploader = table.Column<bool>(type: "INTEGER", nullable: false),
                    LikeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeText = table.Column<string>(type: "TEXT", nullable: false),
                    ParentId = table.Column<string>(type: "TEXT", nullable: false),
                    IsFavorited = table.Column<bool>(type: "INTEGER", nullable: false),
                    YoutubeVideoId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeComment_YoutubeVideos_YoutubeVideoId",
                        column: x => x.YoutubeVideoId,
                        principalTable: "YoutubeVideos",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ApplicationSettings",
                columns: new[] { "Id", "BackupIntervalSeconds", "CheckForNewVideosIntervalSeconds", "CleanUpInterval", "CurrentTask", "DownloadPath", "MaxVideoDuration", "NamingFormat", "UpdateChannelsIntervalSeconds", "UpdateVideosIntervalSeconds", "WorkInterval" },
                values: new object[] { "1", 0L, 0L, 5000L, null, "data/videos", new TimeSpan(0, 1, 0, 0, 0), "{id} - {title}s{ext}", 0L, 0L, 10000L });

            migrationBuilder.CreateIndex(
                name: "IX_QueuedDownloads_VideoId",
                table: "QueuedDownloads",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscribedChannels_ChannelId",
                table: "SubscribedChannels",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeComment_YoutubeVideoId",
                table: "YoutubeComment",
                column: "YoutubeVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_YoutubeChannelId",
                table: "YoutubeVideos",
                column: "YoutubeChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSettings");

            migrationBuilder.DropTable(
                name: "LocalVideos");

            migrationBuilder.DropTable(
                name: "QueuedDownloads");

            migrationBuilder.DropTable(
                name: "SubscribedChannels");

            migrationBuilder.DropTable(
                name: "YoutubeComment");

            migrationBuilder.DropTable(
                name: "YoutubeVideos");

            migrationBuilder.DropTable(
                name: "YoutubeChannels");
        }
    }
}
