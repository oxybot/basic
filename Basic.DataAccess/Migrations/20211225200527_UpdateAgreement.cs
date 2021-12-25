using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.Migrations
{
    public partial class UpdateAgreement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "DefaultPrice",
                table: "Product");

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultUnitPrice",
                table: "Product",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultQuantity",
                table: "Product",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DefaultDescription",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerIdentifier",
                table: "Agreement",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_OwnerIdentifier",
                table: "Agreement",
                column: "OwnerIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreement_User_OwnerIdentifier",
                table: "Agreement",
                column: "OwnerIdentifier",
                principalTable: "User",
                principalColumn: "Identifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreement_User_OwnerIdentifier",
                table: "Agreement");

            migrationBuilder.DropIndex(
                name: "IX_Agreement_OwnerIdentifier",
                table: "Agreement");

            migrationBuilder.DropColumn(
                name: "DefaultDescription",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OwnerIdentifier",
                table: "Agreement");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultUnitPrice",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultQuantity",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AddColumn<string>(
                name: "DefaultPrice",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
