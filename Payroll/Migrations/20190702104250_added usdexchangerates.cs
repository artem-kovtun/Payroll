using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class addedusdexchangerates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsdExchangeRates",
                columns: table => new
                {
                    Date = table.Column<DateTime>(nullable: false),
                    ExchangeRate = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsdExchangeRates", x => x.Date);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsdExchangeRates");
        }
    }
}
