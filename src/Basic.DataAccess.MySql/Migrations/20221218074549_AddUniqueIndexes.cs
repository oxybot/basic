using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    public partial class AddUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Status_DisplayName",
                table: "Status",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Code",
                table: "Role",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_DisplayName",
                table: "Product",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventCategory_DisplayName",
                table: "EventCategory",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_DisplayName",
                table: "Client",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_InternalCode",
                table: "Agreement",
                column: "InternalCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Username",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Status_DisplayName",
                table: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Role_Code",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Product_DisplayName",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_EventCategory_DisplayName",
                table: "EventCategory");

            migrationBuilder.DropIndex(
                name: "IX_Client_DisplayName",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Agreement_InternalCode",
                table: "Agreement");
        }
    }
}
