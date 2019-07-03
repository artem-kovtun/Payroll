using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class Modifieddocumententity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignerId",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkCompletionDate",
                table: "Documents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AssignerId",
                table: "Documents",
                column: "AssignerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Assigners_AssignerId",
                table: "Documents",
                column: "AssignerId",
                principalTable: "Assigners",
                principalColumn: "AssignerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Assigners_AssignerId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_AssignerId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "AssignerId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "WorkCompletionDate",
                table: "Documents");
        }
    }
}
