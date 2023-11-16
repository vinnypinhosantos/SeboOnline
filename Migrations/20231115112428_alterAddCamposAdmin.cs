using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeboOnline.Migrations
{
    /// <inheritdoc />
    public partial class alterAddCamposAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "User",
                type: "NVARCHAR(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "User");
        }
    }
}
