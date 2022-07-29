using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    public partial class MySql3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blob",
                table: "Attachment");

            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "Attachment",
                newName: "AttachmentContent_MimeType");

            migrationBuilder.AddColumn<byte[]>(
                name: "AttachmentContent_Data",
                table: "Attachment",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EventIdentifier",
                table: "Attachment",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_EventIdentifier",
                table: "Attachment",
                column: "EventIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Event_EventIdentifier",
                table: "Attachment",
                column: "EventIdentifier",
                principalTable: "Event",
                principalColumn: "Identifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Event_EventIdentifier",
                table: "Attachment");

            migrationBuilder.DropIndex(
                name: "IX_Attachment_EventIdentifier",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "AttachmentContent_Data",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "EventIdentifier",
                table: "Attachment");

            migrationBuilder.RenameColumn(
                name: "AttachmentContent_MimeType",
                table: "Attachment",
                newName: "Extension");

            migrationBuilder.AddColumn<int>(
                name: "Blob",
                table: "Attachment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
