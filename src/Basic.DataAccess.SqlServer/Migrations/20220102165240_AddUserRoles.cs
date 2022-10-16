// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class AddUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesIdentifier, x.UsersIdentifier });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role_RolesIdentifier",
                        column: x => x.RolesIdentifier,
                        principalTable: "Role",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_UsersIdentifier",
                        column: x => x.UsersIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Identifier", "Code" },
                values: new object[] { new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"), "ClientRO" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Identifier", "Code" },
                values: new object[] { new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"), "Client" });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersIdentifier",
                table: "RoleUser",
                column: "UsersIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
