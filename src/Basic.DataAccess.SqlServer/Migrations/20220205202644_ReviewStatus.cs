using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class ReviewStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Status",
                keyColumn: "Identifier",
                keyValue: new Guid("4151c014-ddde-43e4-aa7e-b98a339bbe74"),
                column: "IsActive",
                value: true);

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Identifier", "Description", "DisplayName", "IsActive" },
                values: new object[] { new Guid("fdac7cc3-3fe0-4e59-ab16-aeaec008f940"), "The associated event has been canceled", "Canceled", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Identifier",
                keyValue: new Guid("fdac7cc3-3fe0-4e59-ab16-aeaec008f940"));

            migrationBuilder.UpdateData(
                table: "Status",
                keyColumn: "Identifier",
                keyValue: new Guid("4151c014-ddde-43e4-aa7e-b98a339bbe74"),
                column: "IsActive",
                value: false);
        }
    }
}
