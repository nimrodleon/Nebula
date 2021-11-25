using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class RemoveSoftDeleteInventoryNoteColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "TransferNoteDetails");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "InventoryNoteDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "TransferNoteDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "InventoryNoteDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
