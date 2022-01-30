using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class UpdateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"),
                column: "Code",
                value: "time");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"),
                column: "Code",
                value: "time-ro");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"),
                column: "Code",
                value: "people");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"),
                column: "Code",
                value: "people-ro");
        }
    }
}
