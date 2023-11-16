using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeboOnline.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldMagazines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Item_DateEdition",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "DateEdition",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Item");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Item",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Item",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEdition",
                table: "Item",
                type: "DATE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "Item",
                type: "NVARCHAR(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Item_DateEdition",
                table: "Item",
                column: "DateEdition");
        }
    }
}
