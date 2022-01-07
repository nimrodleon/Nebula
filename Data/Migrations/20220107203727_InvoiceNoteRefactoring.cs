using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class InvoiceNoteRefactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumDescTotal",
                table: "InvoiceNotes");

            migrationBuilder.DropColumn(
                name: "SumOtrosCargos",
                table: "InvoiceNotes");

            migrationBuilder.DropColumn(
                name: "SumTotalAnticipos",
                table: "InvoiceNotes");

            migrationBuilder.AddColumn<string>(
                name: "DocType",
                table: "InvoiceNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "InvoiceNotes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceType",
                table: "InvoiceNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "InvoiceNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serie",
                table: "InvoiceNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocType",
                table: "InvoiceNotes");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "InvoiceNotes");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "InvoiceNotes");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "InvoiceNotes");

            migrationBuilder.DropColumn(
                name: "Serie",
                table: "InvoiceNotes");

            migrationBuilder.AddColumn<decimal>(
                name: "SumDescTotal",
                table: "InvoiceNotes",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SumOtrosCargos",
                table: "InvoiceNotes",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SumTotalAnticipos",
                table: "InvoiceNotes",
                type: "numeric",
                nullable: true);
        }
    }
}
