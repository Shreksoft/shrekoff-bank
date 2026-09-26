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

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionMoney_Amount",
                table: "Transfers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CommissionMoney_Currency_Code",
                table: "Transfers",
                nullable: false,
                defaultValue: "SLP");

            migrationBuilder.Sql(
                """
                UPDATE "Transfers"
                SET "CommissionMoney_Amount" = "Commission",
                    "CommissionMoney_Currency_Code" = "RecipientMoney_Currency_Code";
                """
            );

            migrationBuilder.DropColumn(
                name: "Commission",
                table: "Transfers"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<decimal>(
                name: "Commission",
                table: "Transfers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m
            );

            migrationBuilder.Sql(
                """
                UPDATE "Transfers"
                SET "Commission" = "CommissionMoney_Amount"
                """
            );

            migrationBuilder.DropColumn(
                name: "CommissionMoney_Amount",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "CommissionMoney_Currency_Code",
                table: "Transfers");

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

            migrationBuilder.Sql(
                """
                UPDATE "Transfers"
                SET "SenderSide_Currency_Scale" = CASE "SenderSide_Currency_Code" WHEN 'SLP' THEN 2 ELSE 5 END,
                "RecipientSide_Currency_Scale" = CASE "RecipientSide_Currency_Code" WHEN 'SLP' THEN 2 ELSE 5 END;
                UPDATE "Accounts"
                SET "Money_Currency_Scale" = CASE "Money_Currency_Code" WHEN 'SLP' THEN 2 ELSE 5 END;
                """
            );
        }
    }
}
