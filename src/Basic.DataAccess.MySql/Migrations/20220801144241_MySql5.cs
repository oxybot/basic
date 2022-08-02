using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    public partial class MySql5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Event_EventIdentifier",
                table: "Attachment");

            migrationBuilder.RenameColumn(
                name: "EventIdentifier",
                table: "Attachment",
                newName: "EventobjIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_Attachment_EventIdentifier",
                table: "Attachment",
                newName: "IX_Attachment_EventobjIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Event_EventobjIdentifier",
                table: "Attachment",
                column: "EventobjIdentifier",
                principalTable: "Event",
                principalColumn: "Identifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Event_EventobjIdentifier",
                table: "Attachment");

            migrationBuilder.RenameColumn(
                name: "EventobjIdentifier",
                table: "Attachment",
                newName: "EventIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_Attachment_EventobjIdentifier",
                table: "Attachment",
                newName: "IX_Attachment_EventIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Event_EventIdentifier",
                table: "Attachment",
                column: "EventIdentifier",
                principalTable: "Event",
                principalColumn: "Identifier");
        }
    }
}
