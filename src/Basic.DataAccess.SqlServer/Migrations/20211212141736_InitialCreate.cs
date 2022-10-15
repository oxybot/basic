using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultPrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultUnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultQuantity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "ClientContract",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientContract", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_ClientContract_Client_ClientIdentifier",
                        column: x => x.ClientIdentifier,
                        principalTable: "Client",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientContractIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Invoice_ClientContract_ClientContractIdentifier",
                        column: x => x.ClientContractIdentifier,
                        principalTable: "ClientContract",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Service_ClientContract_ContractIdentifier",
                        column: x => x.ContractIdentifier,
                        principalTable: "ClientContract",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Service_Product_ProductIdentifier",
                        column: x => x.ProductIdentifier,
                        principalTable: "Product",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientContract_ClientIdentifier",
                table: "ClientContract",
                column: "ClientIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ClientContractIdentifier",
                table: "Invoice",
                column: "ClientContractIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ContractIdentifier",
                table: "Service",
                column: "ContractIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ProductIdentifier",
                table: "Service",
                column: "ProductIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ClientContract");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
