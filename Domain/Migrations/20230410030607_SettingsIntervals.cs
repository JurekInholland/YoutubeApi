using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class SettingsIntervals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CleanUpInterval",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WorkInterval",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "ApplicationSettings",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "CleanUpInterval", "WorkInterval" },
                values: new object[] { 5000L, 1000L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CleanUpInterval",
                table: "ApplicationSettings");

            migrationBuilder.DropColumn(
                name: "WorkInterval",
                table: "ApplicationSettings");
        }
    }
}
