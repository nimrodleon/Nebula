﻿using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class AddInvoiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeDoc = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipOperacion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FecEmision = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    HorEmision = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FecVencimiento = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodLocalEmisor = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipDocUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NumDocUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    RznSocialUsuario = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    TipMoneda = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SumTotTributos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotValVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SumPrecioVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SumDescTotal = table.Column<decimal>(type: "numeric", nullable: true),
                    SumOtrosCargos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotalAnticipos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumImpVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_InvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}