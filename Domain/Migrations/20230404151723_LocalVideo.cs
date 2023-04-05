using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class LocalVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalVideoId",
                table: "YoutubeVideos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocalVideo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    Extension = table.Column<string>(type: "TEXT", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Fps = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Vbr = table.Column<float>(type: "REAL", nullable: false),
                    Abr = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalVideo", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ApplicationSettings",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "DownloadPath", "MaxVideoDuration" },
                values: new object[] { "data/videos", new TimeSpan(0, 1, 0, 0, 0) });

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_LocalVideoId",
                table: "YoutubeVideos",
                column: "LocalVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_LocalVideo_LocalVideoId",
                table: "YoutubeVideos",
                column: "LocalVideoId",
                principalTable: "LocalVideo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_LocalVideo_LocalVideoId",
                table: "YoutubeVideos");

            migrationBuilder.DropTable(
                name: "LocalVideo");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeVideos_LocalVideoId",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "LocalVideoId",
                table: "YoutubeVideos");

            migrationBuilder.UpdateData(
                table: "ApplicationSettings",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "DownloadPath", "MaxVideoDuration" },
                values: new object[] { "C:\\Users\\james\\Downloads\\Youtube", new TimeSpan(0, 0, 0, 0, 0) });
        }
    }
}
