using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.Data.Migrations
{
    public partial class FkAddOriginAndTargetTransferNoteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origin",
                table: "TransferNotes");

            migrationBuilder.DropColumn(
                name: "Target",
                table: "TransferNotes");

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

            migrationBuilder.CreateIndex(
                name: "IX_TransferNotes_OriginId",
                table: "TransferNotes",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferNotes_TargetId",
                table: "TransferNotes",
                column: "TargetId");

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

            migrationBuilder.DropIndex(
                name: "IX_TransferNotes_OriginId",
                table: "TransferNotes");

            migrationBuilder.DropIndex(
                name: "IX_TransferNotes_TargetId",
                table: "TransferNotes");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "TransferNotes");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "TransferNotes");

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "TransferNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Target",
                table: "TransferNotes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
