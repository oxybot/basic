using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    public partial class MySql02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Event_EventIdentifier",
                table: "Attachment");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventIdentifier",
                table: "Attachment",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Event_EventIdentifier",
                table: "Attachment",
                column: "EventIdentifier",
                principalTable: "Event",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Event_EventIdentifier",
                table: "Attachment");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventIdentifier",
                table: "Attachment",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Event_EventIdentifier",
                table: "Attachment",
                column: "EventIdentifier",
                principalTable: "Event",
                principalColumn: "Identifier");
        }
    }
}
