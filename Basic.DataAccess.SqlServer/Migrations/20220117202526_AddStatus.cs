using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class AddStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "AgreementStatus",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgreementIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedByIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementStatus", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_AgreementStatus_Agreement_AgreementIdentifier",
                        column: x => x.AgreementIdentifier,
                        principalTable: "Agreement",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_AgreementStatus_Status_StatusIdentifier",
                        column: x => x.StatusIdentifier,
                        principalTable: "Status",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgreementStatus_User_UpdatedByIdentifier",
                        column: x => x.UpdatedByIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventStatus",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedByIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatus", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_EventStatus_Event_EventIdentifier",
                        column: x => x.EventIdentifier,
                        principalTable: "Event",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_EventStatus_Status_StatusIdentifier",
                        column: x => x.StatusIdentifier,
                        principalTable: "Status",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventStatus_User_UpdatedByIdentifier",
                        column: x => x.UpdatedByIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Identifier", "Description", "DisplayName", "IsActive" },
                values: new object[] { new Guid("4151c014-ddde-43e4-aa7e-b98a339bbe74"), "The associated event has been approved", "Approved", false });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Identifier", "Description", "DisplayName", "IsActive" },
                values: new object[] { new Guid("52bc6354-d8ef-44e2-87ca-c64deeeb22e8"), "The associated event has been created and is waiting for approval", "Requested", true });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Identifier", "Description", "DisplayName", "IsActive" },
                values: new object[] { new Guid("e7f8dcc7-57d5-4e74-ac38-1fbd5153996c"), "The associated event has been rejected", "Rejected", false });

            migrationBuilder.CreateIndex(
                name: "IX_AgreementStatus_AgreementIdentifier",
                table: "AgreementStatus",
                column: "AgreementIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_AgreementStatus_StatusIdentifier",
                table: "AgreementStatus",
                column: "StatusIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_AgreementStatus_UpdatedByIdentifier",
                table: "AgreementStatus",
                column: "UpdatedByIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EventStatus_EventIdentifier",
                table: "EventStatus",
                column: "EventIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EventStatus_StatusIdentifier",
                table: "EventStatus",
                column: "StatusIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EventStatus_UpdatedByIdentifier",
                table: "EventStatus",
                column: "UpdatedByIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgreementStatus");

            migrationBuilder.DropTable(
                name: "EventStatus");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
