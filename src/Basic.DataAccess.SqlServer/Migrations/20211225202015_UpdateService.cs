// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class UpdateService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Product_ProductIdentifier",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_ProductIdentifier",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ProductIdentifier",
                table: "Service");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductIdentifier",
                table: "Service",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_ProductIdentifier",
                table: "Service",
                column: "ProductIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Product_ProductIdentifier",
                table: "Service",
                column: "ProductIdentifier",
                principalTable: "Product",
                principalColumn: "Identifier");
        }
    }
}
