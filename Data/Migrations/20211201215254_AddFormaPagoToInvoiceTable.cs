using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddFormaPagoToInvoiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodLocalEmisor",
                table: "Invoices",
                newName: "FormaPago");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FormaPago",
                table: "Invoices",
                newName: "CodLocalEmisor");
        }
    }
}
