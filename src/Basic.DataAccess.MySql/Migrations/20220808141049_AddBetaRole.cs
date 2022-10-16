// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    public partial class AddBetaRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Identifier", "Code" },
                values: new object[] { new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"), "beta" });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesIdentifier", "UsersIdentifier" },
                values: new object[] { new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RolesIdentifier", "UsersIdentifier" },
                keyValues: new object[] { new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"));
        }
    }
}
