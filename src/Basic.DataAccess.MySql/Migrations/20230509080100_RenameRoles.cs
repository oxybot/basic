// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    /// <inheritdoc />
    public partial class RenameRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"),
                column: "Code",
                value: "users");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"),
                column: "Code",
                value: "clients");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"),
                column: "Code",
                value: "schedules");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"),
                column: "Code",
                value: "clients-ro");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"),
                column: "Code",
                value: "schedules-ro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"),
                column: "Code",
                value: "user");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"),
                column: "Code",
                value: "client");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"),
                column: "Code",
                value: "time");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"),
                column: "Code",
                value: "client-ro");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"),
                column: "Code",
                value: "time-ro");
        }
    }
}
