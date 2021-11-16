using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class AddInvoiceNoteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipOperacion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FecEmision = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    HorEmision = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodLocalEmisor = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipDocUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NumDocUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    RznSocialUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipMoneda = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodMotivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DesMotivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipDocAfectado = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NumDocAfectado = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SumTotTributos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotValVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SumPrecioVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SumDescTotal = table.Column<decimal>(type: "numeric", nullable: true),
                    SumOtrosCargos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotalAnticipos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumImpVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceNoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceNoteId = table.Column<int>(type: "integer", nullable: true),
                    CodUnidadMedida = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CtdUnidadItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CodProducto = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodProductoSunat = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DesItem = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MtoValorUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotTributosItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CodTriIgv = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoIgvItem = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoBaseIgvItem = table.Column<decimal>(type: "numeric", nullable: true),
                    NomTributoIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTipTributoIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipAfeIgv = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PorIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTriIcbper = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoTriIcbperItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CtdBolsasTriIcbperItem = table.Column<int>(type: "integer", nullable: true),
                    NomTributoIcbperItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTipTributoIcbperItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoTriIcbperUnidad = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoPrecioVentaUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoValorVentaItem = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoValorReferencialUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceNoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceNoteDetails_InvoiceNotes_InvoiceNoteId",
                        column: x => x.InvoiceNoteId,
                        principalTable: "InvoiceNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceNoteDetails_InvoiceNoteId",
                table: "InvoiceNoteDetails",
                column: "InvoiceNoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceNoteDetails");

            migrationBuilder.DropTable(
                name: "InvoiceNotes");
        }
    }
}
