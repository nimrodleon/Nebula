using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddInvoiceIdCashierDetailColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "CashierDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashierDetails_InvoiceId",
                table: "CashierDetails",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashierDetails_Invoices_InvoiceId",
                table: "CashierDetails",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashierDetails_Invoices_InvoiceId",
                table: "CashierDetails");

            migrationBuilder.DropIndex(
                name: "IX_CashierDetails_InvoiceId",
                table: "CashierDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "CashierDetails");
        }
    }
}
