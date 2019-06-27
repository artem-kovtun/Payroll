using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    VAT = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    USDRate = table.Column<float>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Middlename = table.Column<string>(nullable: true),
                    FirstnameInAblative = table.Column<string>(nullable: true),
                    LastnameInAblative = table.Column<string>(nullable: true),
                    MiddlenameInAblative = table.Column<string>(nullable: true),
                    ContractNumber = table.Column<int>(nullable: false),
                    ContractDate = table.Column<DateTime>(nullable: false),
                    AddressIndex = table.Column<int>(nullable: false),
                    AddressStreet = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    RecipientBank = table.Column<string>(nullable: true),
                    RegisterNumber = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.VAT);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
