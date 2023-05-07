// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_User_UsersIdentifier",
                table: "RoleUser");

            migrationBuilder.RenameColumn(
                name: "UsersIdentifier",
                table: "RoleUser",
                newName: "UserIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_UsersIdentifier",
                table: "RoleUser",
                newName: "IX_RoleUser_UserIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_User_UserIdentifier",
                table: "RoleUser",
                column: "UserIdentifier",
                principalTable: "User",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_User_UserIdentifier",
                table: "RoleUser");

            migrationBuilder.RenameColumn(
                name: "UserIdentifier",
                table: "RoleUser",
                newName: "UsersIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_UserIdentifier",
                table: "RoleUser",
                newName: "IX_RoleUser_UsersIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_User_UsersIdentifier",
                table: "RoleUser",
                column: "UsersIdentifier",
                principalTable: "User",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
