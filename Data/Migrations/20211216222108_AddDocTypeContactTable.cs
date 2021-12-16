using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddDocTypeContactTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "DocType",
                table: "Contacts",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocType",
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
    }
}
