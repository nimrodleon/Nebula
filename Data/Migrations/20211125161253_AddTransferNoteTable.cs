using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class AddTransferNoteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Origin = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Target = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferNoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferNoteId = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferNoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferNoteDetails_TransferNotes_TransferNoteId",
                        column: x => x.TransferNoteId,
                        principalTable: "TransferNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferNoteDetails_TransferNoteId",
                table: "TransferNoteDetails",
                column: "TransferNoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferNoteDetails");

            migrationBuilder.DropTable(
                name: "TransferNotes");
        }
    }
}
