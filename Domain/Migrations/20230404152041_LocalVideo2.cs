using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class LocalVideo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_LocalVideo_LocalVideoId",
                table: "YoutubeVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalVideo",
                table: "LocalVideo");

            migrationBuilder.RenameTable(
                name: "LocalVideo",
                newName: "LocalVideos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalVideos",
                table: "LocalVideos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_LocalVideos_LocalVideoId",
                table: "YoutubeVideos",
                column: "LocalVideoId",
                principalTable: "LocalVideos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_LocalVideos_LocalVideoId",
                table: "YoutubeVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalVideos",
                table: "LocalVideos");

            migrationBuilder.RenameTable(
                name: "LocalVideos",
                newName: "LocalVideo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalVideo",
                table: "LocalVideo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_LocalVideo_LocalVideoId",
                table: "YoutubeVideos",
                column: "LocalVideoId",
                principalTable: "LocalVideo",
                principalColumn: "Id");
        }
    }
}
