using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class AddCashierDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashierDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CajaDiariaId = table.Column<int>(type: "integer", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Document = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Contact = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Glosa = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PaymentType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Total = table.Column<decimal>(type: "numeric", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashierDetails_CajasDiaria_CajaDiariaId",
                        column: x => x.CajaDiariaId,
                        principalTable: "CajasDiaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashierDetails_CajaDiariaId",
                table: "CashierDetails",
                column: "CajaDiariaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashierDetails");
        }
    }
}
