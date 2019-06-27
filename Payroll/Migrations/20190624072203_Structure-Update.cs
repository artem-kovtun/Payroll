using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class StructureUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Username");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Hours = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_Services_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    VAT = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_UserProfiles", x => x.VAT);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_Username",
                table: "Services",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Username",
                table: "UserProfiles",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    VAT = table.Column<string>(nullable: false),
                    AccountNumber = table.Column<string>(nullable: true),
                    AddressIndex = table.Column<int>(nullable: false),
                    AddressStreet = table.Column<string>(nullable: true),
                    ContractDate = table.Column<DateTime>(nullable: false),
                    ContractNumber = table.Column<int>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    FirstnameInAblative = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    LastnameInAblative = table.Column<string>(nullable: true),
                    Middlename = table.Column<string>(nullable: true),
                    MiddlenameInAblative = table.Column<string>(nullable: true),
                    RecipientBank = table.Column<string>(nullable: true),
                    RegisterNumber = table.Column<string>(nullable: true),
                    USDRate = table.Column<float>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
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
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId");
        }
    }
}
