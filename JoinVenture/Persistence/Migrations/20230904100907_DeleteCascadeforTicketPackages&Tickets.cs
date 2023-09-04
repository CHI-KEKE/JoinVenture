using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCascadeforTicketPackagesTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketPackages_TicketPackageId",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketPackages_TicketPackageId",
                table: "Tickets",
                column: "TicketPackageId",
                principalTable: "TicketPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketPackages_TicketPackageId",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketPackages_TicketPackageId",
                table: "Tickets",
                column: "TicketPackageId",
                principalTable: "TicketPackages",
                principalColumn: "Id");
        }
    }
}
