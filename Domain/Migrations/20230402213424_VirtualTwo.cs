using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class VirtualTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueuedDownloads_YoutubeVideos_YoutubeVideoId",
                table: "QueuedDownloads");

            migrationBuilder.DropIndex(
                name: "IX_QueuedDownloads_YoutubeVideoId",
                table: "QueuedDownloads");

            migrationBuilder.DropColumn(
                name: "YoutubeVideoId",
                table: "QueuedDownloads");

            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "QueuedDownloads",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueuedDownloads_VideoId",
                table: "QueuedDownloads",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_QueuedDownloads_YoutubeVideos_VideoId",
                table: "QueuedDownloads",
                column: "VideoId",
                principalTable: "YoutubeVideos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueuedDownloads_YoutubeVideos_VideoId",
                table: "QueuedDownloads");

            migrationBuilder.DropIndex(
                name: "IX_QueuedDownloads_VideoId",
                table: "QueuedDownloads");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "QueuedDownloads");

            migrationBuilder.AddColumn<string>(
                name: "YoutubeVideoId",
                table: "QueuedDownloads",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_QueuedDownloads_YoutubeVideoId",
                table: "QueuedDownloads",
                column: "YoutubeVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_QueuedDownloads_YoutubeVideos_YoutubeVideoId",
                table: "QueuedDownloads",
                column: "YoutubeVideoId",
                principalTable: "YoutubeVideos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
