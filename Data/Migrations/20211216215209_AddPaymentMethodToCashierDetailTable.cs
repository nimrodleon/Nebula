using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class AddPaymentMethodToCashierDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "CashierDetails",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "CashierDetails");
        }
    }
}
