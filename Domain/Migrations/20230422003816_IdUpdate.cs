using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class IdUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "YoutubeChannels",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "YoutubeVideoId",
                table: "LocalVideos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LocalVideos_YoutubeVideoId",
                table: "LocalVideos",
                column: "YoutubeVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalVideos_YoutubeVideos_YoutubeVideoId",
                table: "LocalVideos",
                column: "YoutubeVideoId",
                principalTable: "YoutubeVideos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalVideos_YoutubeVideos_YoutubeVideoId",
                table: "LocalVideos");

            migrationBuilder.DropIndex(
                name: "IX_LocalVideos_YoutubeVideoId",
                table: "LocalVideos");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "YoutubeChannels");

            migrationBuilder.DropColumn(
                name: "YoutubeVideoId",
                table: "LocalVideos");
        }
    }
}
