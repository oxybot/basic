using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    public partial class MySqlInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address_Line1 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address_Line2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address_PostalCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address_City = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address_Country = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Identifier);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventCategory",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequireBalance = table.Column<bool>(type: "bit", nullable: false),
                    ColorClass = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mapping = table.Column<string>(type: "nvarchar(24)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategory", x => x.Identifier);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GlobalDayOff",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalDayOff", x => x.Identifier);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultUnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DefaultQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Identifier);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Identifier);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Identifier);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salt = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar_Data = table.Column<byte[]>(type: "longblob", nullable: true),
                    Avatar_MimeType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Identifier);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Agreement",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClientIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InternalCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OwnerIdentifier = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SignatureDate = table.Column<DateTime>(type: "date", nullable: true),
                    PrivateNotes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreement", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Agreement_Client_ClientIdentifier",
                        column: x => x.ClientIdentifier,
                        principalTable: "Client",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreement_User_OwnerIdentifier",
                        column: x => x.OwnerIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Allowed = table.Column<int>(type: "int", nullable: false),
                    Transfered = table.Column<int>(type: "int", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    DurationFirstDay = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DurationLastDay = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DurationTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsersIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesIdentifier, x.UsersIdentifier });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role_RolesIdentifier",
                        column: x => x.RolesIdentifier,
                        principalTable: "Role",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_UsersIdentifier",
                        column: x => x.UsersIdentifier,
                        principalTable: "User",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ActiveFrom = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActiveTo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkingSchedule = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AgreementItem",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgreementIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductIdentifier = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AgreementStatus",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgreementIdentifier = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StatusIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpdatedByIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgreementIdentifier = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Invoice_Agreement_AgreementIdentifier",
                        column: x => x.AgreementIdentifier,
                        principalTable: "Agreement",
                        principalColumn: "Identifier");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventStatus",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EventIdentifier = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StatusIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpdatedByIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Identifier", "Code" },
                values: new object[,]
                {
                    { new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"), "user" },
                    { new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"), "client" },
                    { new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"), "time" },
                    { new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"), "client-ro" },
                    { new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"), "time-ro" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Identifier", "Description", "DisplayName", "IsActive" },
                values: new object[,]
                {
                    { new Guid("4151c014-ddde-43e4-aa7e-b98a339bbe74"), "The associated event has been approved", "Approved", false },
                    { new Guid("52bc6354-d8ef-44e2-87ca-c64deeeb22e8"), "The associated event has been created and is waiting for approval", "Requested", true },
                    { new Guid("e7f8dcc7-57d5-4e74-ac38-1fbd5153996c"), "The associated event has been rejected", "Rejected", false }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Identifier", "DisplayName", "Password", "Salt", "Title", "Username" },
                values: new object[] { new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5"), "John Doe", "QBG6AuURBMZ4wxp2pERIWzjzhl5QTYnDoKgLQ5uxojc=", "demo", "User Group Evangelist", "demo" });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesIdentifier", "UsersIdentifier" },
                values: new object[,]
                {
                    { new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
                    { new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
                    { new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
                    { new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") },
                    { new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"), new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_ClientIdentifier",
                table: "Agreement",
                column: "ClientIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_OwnerIdentifier",
                table: "Agreement",
                column: "OwnerIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_AgreementItem_AgreementIdentifier",
                table: "AgreementItem",
                column: "AgreementIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_AgreementItem_ProductIdentifier",
                table: "AgreementItem",
                column: "ProductIdentifier");

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

            migrationBuilder.CreateIndex(
                name: "IX_GlobalDayOff_Date",
                table: "GlobalDayOff",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_AgreementIdentifier",
                table: "Invoice",
                column: "AgreementIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersIdentifier",
                table: "RoleUser",
                column: "UsersIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_UserIdentifier",
                table: "Schedule",
                column: "UserIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgreementItem");

            migrationBuilder.DropTable(
                name: "AgreementStatus");

            migrationBuilder.DropTable(
                name: "Balance");

            migrationBuilder.DropTable(
                name: "EventStatus");

            migrationBuilder.DropTable(
                name: "GlobalDayOff");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Agreement");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "EventCategory");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
