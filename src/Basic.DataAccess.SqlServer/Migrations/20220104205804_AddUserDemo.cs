// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class AddUserDemo : Migration
    {
        [SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "Auto generated content")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Identifier", "DisplayName", "Password", "Title", "Username" },
                values: new object[] { new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5"), "John Doe", "demo", "User Group Evangelist", "demo" });

            var relations = new object[,]
            {
                { new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
                { new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
                { new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
                { new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
                { new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
            };
            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesIdentifier", "UsersIdentifier" },
                values: relations);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RolesIdentifier", "UsersIdentifier" },
                keyValues: new object[] { new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RolesIdentifier", "UsersIdentifier" },
                keyValues: new object[] { new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RolesIdentifier", "UsersIdentifier" },
                keyValues: new object[] { new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RolesIdentifier", "UsersIdentifier" },
                keyValues: new object[] { new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RolesIdentifier", "UsersIdentifier" },
                keyValues: new object[] { new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Identifier",
                keyValue: new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5"));
        }
    }
}
