using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class AddInventoryNoteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "TransferNoteDetails");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "TransferNotes",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "Origin",
                table: "TransferNotes",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "TransferNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OriginId",
                table: "TransferNotes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TargetId",
                table: "TransferNotes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InventoryNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactId = table.Column<int>(type: "integer", nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: true),
                    NoteType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Status = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryNotes_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryNotes_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryNoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryNoteId = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryNoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryNoteDetails_InventoryNotes_InventoryNoteId",
                        column: x => x.InventoryNoteId,
                        principalTable: "InventoryNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferNotes_OriginId",
                table: "TransferNotes",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferNotes_TargetId",
                table: "TransferNotes",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryNoteDetails_InventoryNoteId",
                table: "InventoryNoteDetails",
                column: "InventoryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryNotes_ContactId",
                table: "InventoryNotes",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryNotes_WarehouseId",
                table: "InventoryNotes",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferNotes_Warehouses_OriginId",
                table: "TransferNotes",
                column: "OriginId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferNotes_Warehouses_TargetId",
                table: "TransferNotes",
                column: "TargetId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferNotes_Warehouses_OriginId",
                table: "TransferNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferNotes_Warehouses_TargetId",
                table: "TransferNotes");

            migrationBuilder.DropTable(
                name: "InventoryNoteDetails");

            migrationBuilder.DropTable(
                name: "InventoryNotes");

            migrationBuilder.DropIndex(
                name: "IX_TransferNotes_OriginId",
                table: "TransferNotes");

            migrationBuilder.DropIndex(
                name: "IX_TransferNotes_TargetId",
                table: "TransferNotes");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "TransferNotes");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "TransferNotes");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "TransferNotes");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "TransferNotes",
                newName: "Target");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TransferNotes",
                newName: "Origin");

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "TransferNoteDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
