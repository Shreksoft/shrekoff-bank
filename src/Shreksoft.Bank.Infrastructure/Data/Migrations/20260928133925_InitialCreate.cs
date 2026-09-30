using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shreksoft.Bank.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Info_Email = table.Column<string>(type: "text", nullable: true),
                    Info_PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Info_BirthDate_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Info_FullName_FirstName = table.Column<string>(type: "text", nullable: false),
                    Info_FullName_LastName = table.Column<string>(type: "text", nullable: false),
                    Info_FullName_MiddleName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    CommissionMoney_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CommissionMoney_Currency_Code = table.Column<string>(type: "text", nullable: false),
                    RecipientMoney_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RecipientMoney_Currency_Code = table.Column<string>(type: "text", nullable: false),
                    RecipientSide_AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientSide_ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderMoney_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    SenderMoney_Currency_Code = table.Column<string>(type: "text", nullable: false),
                    SenderSide_AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderSide_ClientId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    Money_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Money_Currency_Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ClientId",
                table: "Accounts",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
