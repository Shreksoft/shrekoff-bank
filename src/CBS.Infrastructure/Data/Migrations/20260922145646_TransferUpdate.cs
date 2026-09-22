using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CBS.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransferUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SenderAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    RecipientAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Commission = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rate = table.Column<decimal>(type: "TEXT", nullable: false),
                    RecipientSide_AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecipientSide_ClientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecipientSide_Currency_Code = table.Column<string>(type: "TEXT", nullable: false),
                    RecipientSide_Currency_Scale = table.Column<byte>(type: "INTEGER", nullable: false),
                    SenderSide_AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SenderSide_ClientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SenderSide_Currency_Code = table.Column<string>(type: "TEXT", nullable: false),
                    SenderSide_Currency_Scale = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
