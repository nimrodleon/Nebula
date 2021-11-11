using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddTypeOperationSunatTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeOperationSunat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOperationSunat", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeOperationSunat");
        }
    }
}
