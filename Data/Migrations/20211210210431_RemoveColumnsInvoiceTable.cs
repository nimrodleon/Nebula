using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class RemoveColumnsInvoiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumDescTotal",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SumOtrosCargos",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SumTotalAnticipos",
                table: "Invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SumDescTotal",
                table: "Invoices",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SumOtrosCargos",
                table: "Invoices",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SumTotalAnticipos",
                table: "Invoices",
                type: "numeric",
                nullable: true);
        }
    }
}
