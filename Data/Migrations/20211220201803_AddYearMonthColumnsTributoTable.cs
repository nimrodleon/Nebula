using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddYearMonthColumnsTributoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "Tributos",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "Tributos",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Tributos");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Tributos");
        }
    }
}
