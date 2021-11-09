using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class ChangeDocTypeSerieInvoiceColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeDoc",
                table: "SerieInvoices",
                newName: "DocType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocType",
                table: "SerieInvoices",
                newName: "TypeDoc");
        }
    }
}
