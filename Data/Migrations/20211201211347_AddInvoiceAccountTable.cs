using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class AddInvoiceAccountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MtoValorReferencialUnitario",
                table: "InvoiceDetails");

            migrationBuilder.CreateTable(
                name: "InvoiceAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    Serie = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Number = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AccountType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Status = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Balance = table.Column<decimal>(type: "numeric", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceAccounts_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAccounts_InvoiceId",
                table: "InvoiceAccounts",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceAccounts");

            migrationBuilder.AddColumn<decimal>(
                name: "MtoValorReferencialUnitario",
                table: "InvoiceDetails",
                type: "numeric",
                nullable: true);
        }
    }
}
