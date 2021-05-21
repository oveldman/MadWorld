using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations.Authentication
{
    public partial class AddTwoFactorOptionOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorOn",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorOn",
                table: "AspNetUsers");
        }
    }
}
