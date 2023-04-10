using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class SettingsIntervals2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BackupIntervalSeconds",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CheckForNewVideosIntervalSeconds",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdateChannelsIntervalSeconds",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdateVideosIntervalSeconds",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "ApplicationSettings",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "BackupIntervalSeconds", "CheckForNewVideosIntervalSeconds", "UpdateChannelsIntervalSeconds", "UpdateVideosIntervalSeconds" },
                values: new object[] { 0L, 0L, 0L, 0L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackupIntervalSeconds",
                table: "ApplicationSettings");

            migrationBuilder.DropColumn(
                name: "CheckForNewVideosIntervalSeconds",
                table: "ApplicationSettings");

            migrationBuilder.DropColumn(
                name: "UpdateChannelsIntervalSeconds",
                table: "ApplicationSettings");

            migrationBuilder.DropColumn(
                name: "UpdateVideosIntervalSeconds",
                table: "ApplicationSettings");
        }
    }
}
