using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    public partial class MySql3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DisplayName = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Blob = table.Column<int>(type: "int", nullable: false),
                    Extension = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventIdentifier = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Attachment_Event_EventIdentifier",
                        column: x => x.EventIdentifier,
                        principalTable: "Event",
                        principalColumn: "Identifier");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_EventIdentifier",
                table: "Attachment",
                column: "EventIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");
        }
    }
}
