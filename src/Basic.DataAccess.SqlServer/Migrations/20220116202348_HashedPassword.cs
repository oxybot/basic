// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class HashedPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Identifier",
                keyValue: new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5"),
                columns: new[] { "Password", "Salt" },
                values: new object[] { "QBG6AuURBMZ4wxp2pERIWzjzhl5QTYnDoKgLQ5uxojc=", "demo" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Identifier",
                keyValue: new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5"),
                column: "Password",
                value: "demo");
        }
    }
}
