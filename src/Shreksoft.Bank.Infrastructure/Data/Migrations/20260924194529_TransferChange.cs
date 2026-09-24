using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shreksoft.Bank.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransferChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientSide_Currency_Scale",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SenderSide_Currency_Scale",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Money_Currency_Scale",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "SenderSide_Currency_Code",
                table: "Transfers",
                newName: "SenderMoney_Currency_Code");

            migrationBuilder.RenameColumn(
                name: "RecipientSide_Currency_Code",
                table: "Transfers",
                newName: "RecipientMoney_Currency_Code");

            migrationBuilder.RenameColumn(
                name: "SenderAmount",
                table: "Transfers",
                newName: "SenderMoney_Amount");

            migrationBuilder.RenameColumn(
                name: "RecipientAmount",
                table: "Transfers",
                newName: "RecipientMoney_Amount");

            migrationBuilder.RenameColumn(
                name: "Commission",
                table: "Transfers",
                newName: "CommissionMoney_Currency_Code");

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionMoney_Amount",
                table: "Transfers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionMoney_Amount",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "SenderMoney_Currency_Code",
                table: "Transfers",
                newName: "SenderSide_Currency_Code");

            migrationBuilder.RenameColumn(
                name: "RecipientMoney_Currency_Code",
                table: "Transfers",
                newName: "RecipientSide_Currency_Code");

            migrationBuilder.RenameColumn(
                name: "SenderMoney_Amount",
                table: "Transfers",
                newName: "SenderAmount");

            migrationBuilder.RenameColumn(
                name: "RecipientMoney_Amount",
                table: "Transfers",
                newName: "RecipientAmount");

            migrationBuilder.RenameColumn(
                name: "CommissionMoney_Currency_Code",
                table: "Transfers",
                newName: "Commission");

            migrationBuilder.AddColumn<byte>(
                name: "RecipientSide_Currency_Scale",
                table: "Transfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "SenderSide_Currency_Scale",
                table: "Transfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Money_Currency_Scale",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
