// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class UpdateAgreementItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.CreateTable(
                name: "AgreementItem",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgreementIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementItem", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_AgreementItem_Agreement_AgreementIdentifier",
                        column: x => x.AgreementIdentifier,
                        principalTable: "Agreement",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgreementItem_Product_ProductIdentifier",
                        column: x => x.ProductIdentifier,
                        principalTable: "Product",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgreementItem_AgreementIdentifier",
                table: "AgreementItem",
                column: "AgreementIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_AgreementItem_ProductIdentifier",
                table: "AgreementItem",
                column: "ProductIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgreementItem");

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgreementIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Service_Agreement_AgreementIdentifier",
                        column: x => x.AgreementIdentifier,
                        principalTable: "Agreement",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_AgreementIdentifier",
                table: "Service",
                column: "AgreementIdentifier");
        }
    }
}
