using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.Migrations
{
    public partial class AddUserAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar_Data",
                table: "User",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar_MimeType",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar_Data",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Avatar_MimeType",
                table: "User");
        }
    }
}
