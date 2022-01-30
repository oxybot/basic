using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class UpdateClientContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ClientContract",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "ClientContract",
                newName: "InternalCode");

            migrationBuilder.AddColumn<string>(
                name: "PrivateNotes",
                table: "ClientContract",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SignatureDate",
                table: "ClientContract",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateNotes",
                table: "ClientContract");

            migrationBuilder.DropColumn(
                name: "SignatureDate",
                table: "ClientContract");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ClientContract",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "InternalCode",
                table: "ClientContract",
                newName: "Code");
        }
    }
}
