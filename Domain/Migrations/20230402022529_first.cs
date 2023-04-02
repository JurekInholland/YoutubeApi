using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelatedVideo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelTitle = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Thumbnail = table.Column<string>(type: "TEXT", nullable: false),
                    PublishTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LikeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedVideo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeVideos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Thumbnail = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Uploader = table.Column<string>(type: "TEXT", nullable: false),
                    UploaderId = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationString = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelUrl = table.Column<string>(type: "TEXT", nullable: false),
                    WebpageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ViewCount = table.Column<long>(type: "INTEGER", nullable: false),
                    LikeCount = table.Column<long>(type: "INTEGER", nullable: false),
                    Categories = table.Column<string>(type: "TEXT", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Fps = table.Column<int>(type: "INTEGER", nullable: false),
                    FileSize = table.Column<int>(type: "INTEGER", nullable: false),
                    Vbr = table.Column<float>(type: "REAL", nullable: false),
                    Abr = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeVideos", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_QueuedDownloads_VideoId",
                table: "QueuedDownloads",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeComment_YoutubeVideoId",
                table: "YoutubeComment",
                column: "YoutubeVideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueuedDownloads");

            migrationBuilder.DropTable(
                name: "RelatedVideo");

            migrationBuilder.DropTable(
                name: "YoutubeComment");

            migrationBuilder.DropTable(
                name: "YoutubeVideos");
        }
    }
}
