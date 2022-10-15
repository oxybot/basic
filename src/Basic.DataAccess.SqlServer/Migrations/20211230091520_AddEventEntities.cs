using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class AddEventEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCategory",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequireBalance = table.Column<bool>(type: "bit", nullable: false),
                    Mapping = table.Column<string>(type: "nvarchar(24)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategory", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Allowed = table.Column<int>(type: "int", nullable: false),
                    Transfered = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Balance_EventCategory_CategoryIdentifier",
                        column: x => x.CategoryIdentifier,
                        principalTable: "EventCategory",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Balance_User_UserIdentifier",
                        column: x => x.UserIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationFirstDay = table.Column<int>(type: "int", nullable: false),
                    DurationLastDay = table.Column<int>(type: "int", nullable: false),
                    DurationTotal = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Event_EventCategory_CategoryIdentifier",
                        column: x => x.CategoryIdentifier,
                        principalTable: "EventCategory",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_User_UserIdentifier",
                        column: x => x.UserIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Balance_CategoryIdentifier",
                table: "Balance",
                column: "CategoryIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_UserIdentifier",
                table: "Balance",
                column: "UserIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Event_CategoryIdentifier",
                table: "Event",
                column: "CategoryIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Event_UserIdentifier",
                table: "Event",
                column: "UserIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balance");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "EventCategory");
        }
    }
}
