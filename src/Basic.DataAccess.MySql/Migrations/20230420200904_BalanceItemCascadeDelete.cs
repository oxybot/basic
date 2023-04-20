using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
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
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

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
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceItem_Balance_BalanceIdentifier",
                table: "BalanceItem",
                column: "BalanceIdentifier",
                principalTable: "Balance",
                principalColumn: "Identifier");
        }
    }
}
