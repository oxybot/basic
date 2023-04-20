using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class BalanceItemCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BalanceItem_Balance_BalanceIdentifier",
                table: "BalanceItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "BalanceIdentifier",
                table: "BalanceItem",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceItem_Balance_BalanceIdentifier",
                table: "BalanceItem",
                column: "BalanceIdentifier",
                principalTable: "Balance",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BalanceItem_Balance_BalanceIdentifier",
                table: "BalanceItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "BalanceIdentifier",
                table: "BalanceItem",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceItem_Balance_BalanceIdentifier",
                table: "BalanceItem",
                column: "BalanceIdentifier",
                principalTable: "Balance",
                principalColumn: "Identifier");
        }
    }
}
