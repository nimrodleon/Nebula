using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddContactIdColumnToInvoiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "Invoices",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Invoices");
        }
    }
}
