using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class RefactoringCajaDiariaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CajasDiaria_Cajas_CajaId",
                table: "CajasDiaria");

            migrationBuilder.DropTable(
                name: "SerieInvoices");

            migrationBuilder.DropTable(
                name: "Cajas");

            migrationBuilder.RenameColumn(
                name: "CajaId",
                table: "CajasDiaria",
                newName: "InvoiceSerieId");

            migrationBuilder.RenameIndex(
                name: "IX_CajasDiaria_CajaId",
                table: "CajasDiaria",
                newName: "IX_CajasDiaria_InvoiceSerieId");

            migrationBuilder.AddForeignKey(
                name: "FK_CajasDiaria_InvoiceSeries_InvoiceSerieId",
                table: "CajasDiaria",
                column: "InvoiceSerieId",
                principalTable: "InvoiceSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CajasDiaria_InvoiceSeries_InvoiceSerieId",
                table: "CajasDiaria");

            migrationBuilder.RenameColumn(
                name: "InvoiceSerieId",
                table: "CajasDiaria",
                newName: "CajaId");

            migrationBuilder.RenameIndex(
                name: "IX_CajasDiaria_InvoiceSerieId",
                table: "CajasDiaria",
                newName: "IX_CajasDiaria_CajaId");

            migrationBuilder.CreateTable(
                name: "Cajas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cajas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SerieInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CajaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Counter = table.Column<int>(type: "integer", nullable: true),
                    DocType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Prefix = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerieInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SerieInvoices_Cajas_CajaId",
                        column: x => x.CajaId,
                        principalTable: "Cajas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SerieInvoices_CajaId",
                table: "SerieInvoices",
                column: "CajaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CajasDiaria_Cajas_CajaId",
                table: "CajasDiaria",
                column: "CajaId",
                principalTable: "Cajas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
