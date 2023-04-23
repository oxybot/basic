// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    /// <inheritdoc />
    public partial class CurrentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentStatusIdentifier",
                table: "Event",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Event_CurrentStatusIdentifier",
                table: "Event",
                column: "CurrentStatusIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Status_CurrentStatusIdentifier",
                table: "Event",
                column: "CurrentStatusIdentifier",
                principalTable: "Status",
                principalColumn: "Identifier");

            migrationBuilder.Sql(@"
update `Event`
   set `CurrentStatusIdentifier` = (
       select `StatusIdentifier`
         from `EventStatus`
        where `EventStatus`.`EventIdentifier` = `Event`.`Identifier`
     order by `UpdatedOn` desc
        limit 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Status_CurrentStatusIdentifier",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_CurrentStatusIdentifier",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "CurrentStatusIdentifier",
                table: "Event");
        }
    }
}
