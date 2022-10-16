// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class AddBetaRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentContent_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AttachmentContent_MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgreementIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Attachment_Agreement_AgreementIdentifier",
                        column: x => x.AgreementIdentifier,
                        principalTable: "Agreement",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_Attachment_Client_ClientIdentifier",
                        column: x => x.ClientIdentifier,
                        principalTable: "Client",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_Attachment_Event_EventIdentifier",
                        column: x => x.EventIdentifier,
                        principalTable: "Event",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_Attachment_User_UserIdentifier",
                        column: x => x.UserIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier");
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Identifier", "Code" },
                values: new object[] { new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"), "beta" });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesIdentifier", "UsersIdentifier" },
                values: new object[] { new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_AgreementIdentifier",
                table: "Attachment",
                column: "AgreementIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_ClientIdentifier",
                table: "Attachment",
                column: "ClientIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_EventIdentifier",
                table: "Attachment",
                column: "EventIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_UserIdentifier",
                table: "Attachment",
                column: "UserIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DeleteData(
                table: "RoleUser",
                keyColumns: new[] { "RolesIdentifier", "UsersIdentifier" },
                keyValues: new object[] { new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Identifier",
                keyValue: new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"));
        }
    }
}
