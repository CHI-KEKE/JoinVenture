using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrderModelAddTicketsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_OrderId",
                table: "Tickets",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Orders_OrderId",
                table: "Tickets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Orders_OrderId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_OrderId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "TicketId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
