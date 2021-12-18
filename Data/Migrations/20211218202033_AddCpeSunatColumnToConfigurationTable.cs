using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddCpeSunatColumnToConfigurationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletarDatosBoleta",
                table: "Configuration");

            migrationBuilder.AddColumn<string>(
                name: "CpeSunat",
                table: "Configuration",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpeSunat",
                table: "Configuration");

            migrationBuilder.AddColumn<decimal>(
                name: "CompletarDatosBoleta",
                table: "Configuration",
                type: "numeric",
                nullable: true);
        }
    }
}
