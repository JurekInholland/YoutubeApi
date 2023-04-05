using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_LocalVideos_LocalVideoId",
                table: "YoutubeVideos");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeVideos_LocalVideoId",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "LocalVideoId",
                table: "YoutubeVideos");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalVideos_YoutubeVideos_Id",
                table: "LocalVideos",
                column: "Id",
                principalTable: "YoutubeVideos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalVideos_YoutubeVideos_Id",
                table: "LocalVideos");

            migrationBuilder.AddColumn<string>(
                name: "LocalVideoId",
                table: "YoutubeVideos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_LocalVideoId",
                table: "YoutubeVideos",
                column: "LocalVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_LocalVideos_LocalVideoId",
                table: "YoutubeVideos",
                column: "LocalVideoId",
                principalTable: "LocalVideos",
                principalColumn: "Id");
        }
    }
}
