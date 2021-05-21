using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations.Authentication
{
    public partial class AddTwoFactorSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TwoFactorSession",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TwoFactorSessionExpire",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorSession",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorSessionExpire",
                table: "AspNetUsers");
        }
    }
}
