﻿// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class UpdateAttachmentStrategy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.CreateTable(
                name: "AgreementAttachment",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentContent_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AttachmentContent_MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementAttachment", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_AgreementAttachment_Agreement_ParentIdentifier",
                        column: x => x.ParentIdentifier,
                        principalTable: "Agreement",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientAttachment",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentContent_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AttachmentContent_MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAttachment", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_ClientAttachment_Client_ParentIdentifier",
                        column: x => x.ParentIdentifier,
                        principalTable: "Client",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventAttachment",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentContent_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AttachmentContent_MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAttachment", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_EventAttachment_Event_ParentIdentifier",
                        column: x => x.ParentIdentifier,
                        principalTable: "Event",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAttachment",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentContent_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AttachmentContent_MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttachment", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_UserAttachment_User_ParentIdentifier",
                        column: x => x.ParentIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgreementAttachment_ParentIdentifier",
                table: "AgreementAttachment",
                column: "ParentIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAttachment_ParentIdentifier",
                table: "ClientAttachment",
                column: "ParentIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EventAttachment_ParentIdentifier",
                table: "EventAttachment",
                column: "ParentIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachment_ParentIdentifier",
                table: "UserAttachment",
                column: "ParentIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgreementAttachment");

            migrationBuilder.DropTable(
                name: "ClientAttachment");

            migrationBuilder.DropTable(
                name: "EventAttachment");

            migrationBuilder.DropTable(
                name: "UserAttachment");

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentContent_Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AttachmentContent_MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgreementIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
    }
}
