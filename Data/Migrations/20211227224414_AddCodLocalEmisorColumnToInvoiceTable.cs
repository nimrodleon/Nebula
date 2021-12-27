using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddCodLocalEmisorColumnToInvoiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateEnd",
                table: "InvoiceAccounts",
                newName: "EndDate");

            migrationBuilder.AddColumn<string>(
                name: "CodLocalEmisor",
                table: "Invoices",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SumPrecioVenta",
                table: "Invoices",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MtoValorReferencialUnitario",
                table: "InvoiceDetails",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodLocalEmisor",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SumPrecioVenta",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "MtoValorReferencialUnitario",
                table: "InvoiceDetails");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "InvoiceAccounts",
                newName: "DateEnd");
        }
    }
}
