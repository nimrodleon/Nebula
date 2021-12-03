using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddInvoiceSerieTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: true),
                    Factura = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CounterFactura = table.Column<int>(type: "integer", nullable: false),
                    Boleta = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CounterBoleta = table.Column<int>(type: "integer", nullable: false),
                    NotaDeVenta = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CounterNotaDeVenta = table.Column<int>(type: "integer", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceSeries_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSeries_WarehouseId",
                table: "InvoiceSeries",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceSeries");
        }
    }
}
