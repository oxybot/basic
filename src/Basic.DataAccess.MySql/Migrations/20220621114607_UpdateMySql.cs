using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    public partial class UpdateMySql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActiveTo",
                table: "Schedule",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActiveFrom",
                table: "Schedule",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActiveTo",
                table: "Schedule",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActiveFrom",
                table: "Schedule",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "Status",
                keyColumn: "Identifier",
                keyValue: new Guid("4151c014-ddde-43e4-aa7e-b98a339bbe74"),
                column: "IsActive",
                value: false);
        }
    }
}
