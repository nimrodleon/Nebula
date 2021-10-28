using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddPeopleDocTypeIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeDoc",
                table: "Contacts");

            migrationBuilder.AddColumn<Guid>(
                name: "PeopleDocTypeId",
                table: "Contacts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PeopleDocTypeId",
                table: "Contacts",
                column: "PeopleDocTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_PeopleDocTypes_PeopleDocTypeId",
                table: "Contacts",
                column: "PeopleDocTypeId",
                principalTable: "PeopleDocTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_PeopleDocTypes_PeopleDocTypeId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_PeopleDocTypeId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "PeopleDocTypeId",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "TypeDoc",
                table: "Contacts",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
