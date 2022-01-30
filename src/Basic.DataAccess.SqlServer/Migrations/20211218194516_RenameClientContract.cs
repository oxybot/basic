using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.DataAccess.SqlServer.Migrations
{
    public partial class RenameClientContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_ClientContract_ClientContractIdentifier",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_ClientContract_ContractIdentifier",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientContract_Client_ClientIdentifier",
                table: "ClientContract");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientContract",
                table: "ClientContract");

            migrationBuilder.RenameTable(
                name: "ClientContract",
                newName: "Agreement");

            migrationBuilder.RenameColumn(
                name: "ContractIdentifier",
                table: "Service",
                newName: "AgreementIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_Service_ContractIdentifier",
                table: "Service",
                newName: "IX_Service_AgreementIdentifier");

            migrationBuilder.RenameColumn(
                name: "ClientContractIdentifier",
                table: "Invoice",
                newName: "AgreementIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_ClientContractIdentifier",
                table: "Invoice",
                newName: "IX_Invoice_AgreementIdentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agreement",
                table: "Agreement",
                column: "Identifier");

            migrationBuilder.RenameIndex(
                name: "IX_ClientContract_ClientIdentifier",
                table: "Agreement",
                newName: "IX_Agreement_ClientIdentifier");

            migrationBuilder.AddForeignKey(
                 name: "FK_Agreement_Client_ClientIdentifier",
                 table: "Agreement",
                 column: "ClientIdentifier",
                 principalTable: "Client",
                 principalColumn: "Identifier",
                 onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Agreement_AgreementIdentifier",
                table: "Invoice",
                column: "AgreementIdentifier",
                principalTable: "Agreement",
                principalColumn: "Identifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Agreement_AgreementIdentifier",
                table: "Service",
                column: "AgreementIdentifier",
                principalTable: "Agreement",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Agreement_AgreementIdentifier",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Agreement_AgreementIdentifier",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreement_Client_ClientIdentifier",
                table: "Agreement");

            migrationBuilder.RenameIndex(
                name: "IX_Agreement_ClientIdentifier",
                table: "Agreement",
                newName: "IX_ClientContract_ClientIdentifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agreement",
                table: "Agreement");

            migrationBuilder.RenameColumn(
                name: "AgreementIdentifier",
                table: "Service",
                newName: "ContractIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_Service_AgreementIdentifier",
                table: "Service",
                newName: "IX_Service_ContractIdentifier");

            migrationBuilder.RenameColumn(
                name: "AgreementIdentifier",
                table: "Invoice",
                newName: "ClientContractIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_AgreementIdentifier",
                table: "Invoice",
                newName: "IX_Invoice_ClientContractIdentifier");

            migrationBuilder.RenameTable(
                name: "Agreement",
                newName: "ClientContract");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientContract",
                table: "ClientContract",
                column: "Identifier");

            migrationBuilder.AddForeignKey(
                 name: "FK_ClientContract_Client_ClientIdentifier",
                 table: "ClientContract",
                 column: "ClientIdentifier",
                 principalTable: "Client",
                 principalColumn: "Identifier",
                 onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_ClientContract_ClientContractIdentifier",
                table: "Invoice",
                column: "ClientContractIdentifier",
                principalTable: "ClientContract",
                principalColumn: "Identifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ClientContract_ContractIdentifier",
                table: "Service",
                column: "ContractIdentifier",
                principalTable: "ClientContract",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
