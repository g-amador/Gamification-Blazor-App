using Microsoft.EntityFrameworkCore.Migrations;

namespace GamificationAPI.Migrations
{
    public partial class RenameApiSecretToApiPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiSecret",
                table: "Application");

            migrationBuilder.AddColumn<string>(
                name: "ApiPassword",
                table: "Application",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiPassword",
                table: "Application");

            migrationBuilder.AddColumn<string>(
                name: "ApiSecret",
                table: "Application",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
