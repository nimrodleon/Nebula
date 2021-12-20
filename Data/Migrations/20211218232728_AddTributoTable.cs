using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class AddTributoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tributos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    IdeTributo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NomTributo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTipTributo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoBaseImponible = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoTributo = table.Column<decimal>(type: "numeric", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tributos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tributos_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tributos_InvoiceId",
                table: "Tributos",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tributos");
        }
    }
}
