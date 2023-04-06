using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Channel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abr",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "ChannelUrl",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "DurationString",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "Fps",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "Uploader",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "Vbr",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "YoutubeVideos");

            migrationBuilder.RenameColumn(
                name: "UploaderId",
                table: "YoutubeVideos",
                newName: "YoutubeChannelId");

            migrationBuilder.CreateTable(
                name: "YoutubeChannels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeChannels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_YoutubeChannelId",
                table: "YoutubeVideos",
                column: "YoutubeChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_YoutubeChannels_YoutubeChannelId",
                table: "YoutubeVideos",
                column: "YoutubeChannelId",
                principalTable: "YoutubeChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_YoutubeChannels_YoutubeChannelId",
                table: "YoutubeVideos");

            migrationBuilder.DropTable(
                name: "YoutubeChannels");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeVideos_YoutubeChannelId",
                table: "YoutubeVideos");

            migrationBuilder.RenameColumn(
                name: "YoutubeChannelId",
                table: "YoutubeVideos",
                newName: "UploaderId");

            migrationBuilder.AddColumn<float>(
                name: "Abr",
                table: "YoutubeVideos",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ChannelId",
                table: "YoutubeVideos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChannelUrl",
                table: "YoutubeVideos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DurationString",
                table: "YoutubeVideos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FileSize",
                table: "YoutubeVideos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Fps",
                table: "YoutubeVideos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "YoutubeVideos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Uploader",
                table: "YoutubeVideos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Vbr",
                table: "YoutubeVideos",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "YoutubeVideos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
