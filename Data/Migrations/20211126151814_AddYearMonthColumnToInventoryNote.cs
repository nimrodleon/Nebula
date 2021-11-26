using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddYearMonthColumnToInventoryNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "TransferNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "TransferNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "InventoryNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "InventoryNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "TransferNotes");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "TransferNotes");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "InventoryNotes");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "InventoryNotes");
        }
    }
}
