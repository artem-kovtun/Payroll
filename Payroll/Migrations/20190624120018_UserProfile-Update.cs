using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Migrations
{
    public partial class UserProfileUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstnameInAblative",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "MiddlenameInAblative",
                table: "UserProfiles",
                newName: "PaymentPurpose");

            migrationBuilder.RenameColumn(
                name: "LastnameInAblative",
                table: "UserProfiles",
                newName: "FullnameInAblative");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentPurpose",
                table: "UserProfiles",
                newName: "MiddlenameInAblative");

            migrationBuilder.RenameColumn(
                name: "FullnameInAblative",
                table: "UserProfiles",
                newName: "LastnameInAblative");

            migrationBuilder.AddColumn<string>(
                name: "FirstnameInAblative",
                table: "UserProfiles",
                nullable: true);
        }
    }
}
