using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeboOnline.Migrations
{
    /// <inheritdoc />
    public partial class RenameTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Item_IdItem",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_User_IdBuyer",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_User_IdSeller",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_IdSeller",
                table: "Transaction",
                newName: "IX_Transaction_IdSeller");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_IdItem",
                table: "Transaction",
                newName: "IX_Transaction_IdItem");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_IdBuyer",
                table: "Transaction",
                newName: "IX_Transaction_IdBuyer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Item_IdItem",
                table: "Transaction",
                column: "IdItem",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_User_IdBuyer",
                table: "Transaction",
                column: "IdBuyer",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_User_IdSeller",
                table: "Transaction",
                column: "IdSeller",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Item_IdItem",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_IdBuyer",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_IdSeller",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_IdSeller",
                table: "Transactions",
                newName: "IX_Transactions_IdSeller");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_IdItem",
                table: "Transactions",
                newName: "IX_Transactions_IdItem");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_IdBuyer",
                table: "Transactions",
                newName: "IX_Transactions_IdBuyer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Item_IdItem",
                table: "Transactions",
                column: "IdItem",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_User_IdBuyer",
                table: "Transactions",
                column: "IdBuyer",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_User_IdSeller",
                table: "Transactions",
                column: "IdSeller",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
