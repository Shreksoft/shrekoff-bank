using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shreksoft.Bank.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Money_Currency",
                table: "Accounts",
                newName: "Money_Currency_Code");

            migrationBuilder.AddColumn<byte>(
                name: "Money_Currency_Scale",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money_Currency_Scale",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Money_Currency_Code",
                table: "Accounts",
                newName: "Money_Currency");
        }
    }
}
