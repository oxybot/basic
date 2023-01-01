using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class AddOptionsRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Identifier", "Code" },
                values: new object[] { new Guid("e7f909f2-2af9-42d8-bfd0-5ca96022cba2"), "options" });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesIdentifier", "UsersIdentifier" },
                values: new object[] { new Guid("e7f909f2-2af9-42d8-bfd0-5ca96022cba2"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RolesIdentifier", "UsersIdentifier" },
                keyValues: new object[] { new Guid("e7f909f2-2af9-42d8-bfd0-5ca96022cba2"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("e7f909f2-2af9-42d8-bfd0-5ca96022cba2"));
        }
    }
}
