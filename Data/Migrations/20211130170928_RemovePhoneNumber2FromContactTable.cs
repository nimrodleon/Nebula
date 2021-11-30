using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class RemovePhoneNumber2FromContactTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber1",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber2",
                table: "Contacts",
                newName: "PhoneNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Contacts",
                newName: "PhoneNumber2");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber1",
                table: "Contacts",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
