using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ValidatedDateStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValidatedDateEnd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BookingAvailableStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BookingAvailableEnd = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPackages_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_ActivityId",
                table: "TicketPackages",
                column: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketPackages");
        }
    }
}
