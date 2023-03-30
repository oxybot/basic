// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBalanceItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BalanceItem",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    BalanceIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceItem", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_BalanceItem_Balance_BalanceIdentifier",
                        column: x => x.BalanceIdentifier,
                        principalTable: "Balance",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceItem_BalanceIdentifier",
                table: "BalanceItem",
                column: "BalanceIdentifier");

            migrationBuilder.Sql(@"
insert into[BalanceItem] ([BalanceIdentifier], [Order], [Description], [Value])
select[Identifier], 1, ""Allowed"", [Allowed] from[Balance]
where[Transfered] is not null and [Transfered] != 0");

            migrationBuilder.Sql(@"
insert into[BalanceItem] ([BalanceIdentifier], [Order], [Description], [Value])
select[Identifier], 2, ""Transfered"", [Transfered] from[Balance]
where[Transfered] is not null and [Transfered] != 0");

            migrationBuilder.Sql(@"
update[Balance] set[Allowed] = [Allowed] + [Transfered]
where[Transfered] is not null and [Transfered] != 0");

            migrationBuilder.DropColumn(
                name: "Transfered",
                table: "Balance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Transfered",
                table: "Balance",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.DropTable(
                name: "BalanceItem");
        }
    }
}
