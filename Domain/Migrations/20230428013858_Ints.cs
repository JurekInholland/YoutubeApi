using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Ints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subscribers",
                table: "YoutubeChannels");

            migrationBuilder.AlterColumn<int>(
                name: "VideoCount",
                table: "YoutubeChannels",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "SubscriberCount",
                table: "YoutubeChannels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriberCount",
                table: "YoutubeChannels");

            migrationBuilder.AlterColumn<string>(
                name: "VideoCount",
                table: "YoutubeChannels",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Subscribers",
                table: "YoutubeChannels",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
