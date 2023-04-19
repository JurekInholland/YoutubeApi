using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class CurrentTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentTask",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationSettings",
                keyColumn: "Id",
                keyValue: "1",
                column: "CurrentTask",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTask",
                table: "ApplicationSettings");
        }
    }
}
