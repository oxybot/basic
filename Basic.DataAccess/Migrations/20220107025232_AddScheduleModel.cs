using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.Migrations
{
    public partial class AddScheduleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkingSchedule = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Schedule_User_UserIdentifier",
                        column: x => x.UserIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_UserIdentifier",
                table: "Schedule",
                column: "UserIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");
        }
    }
}
