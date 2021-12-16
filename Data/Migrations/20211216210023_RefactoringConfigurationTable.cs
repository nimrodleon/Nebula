using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class RefactoringConfigurationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileControl",
                table: "Configuration",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileSunat",
                table: "Configuration",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlApi",
                table: "Configuration",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileControl",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "FileSunat",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "UrlApi",
                table: "Configuration");
        }
    }
}
