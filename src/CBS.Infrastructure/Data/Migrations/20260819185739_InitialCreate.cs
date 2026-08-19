using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CBS.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsBlocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    Money_Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Money_Currency = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Info_Email = table.Column<string>(type: "TEXT", nullable: true),
                    Info_PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Info_BirthDate_Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Info_FullName_FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    Info_FullName_LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Info_FullName_MiddleName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
