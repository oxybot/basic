// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class AddNewRoles : Migration
    {
        [SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "Auto generated content")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"),
                column: "Code",
                value: "client");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"),
                column: "Code",
                value: "client-ro");

            var roles = new object[,]
                {
                    { new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"), "user" },
                    { new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"), "people" },
                    { new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"), "people-ro" },
                };

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Identifier", "Code" },
                values: roles);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"),
                column: "Code",
                value: "Client");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"),
                column: "Code",
                value: "ClientRO");
        }
    }
}
