using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AppSettingsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationSettings",
                columns: new[] { "Id", "DownloadPath", "MaxVideoDuration", "NamingFormat" },
                values: new object[] { "1", "C:\\Users\\james\\Downloads\\Youtube", new TimeSpan(0, 0, 0, 0, 0), "{id} - {title}s{ext}" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationSettings",
                keyColumn: "Id",
                keyValue: "1");
        }
    }
}
