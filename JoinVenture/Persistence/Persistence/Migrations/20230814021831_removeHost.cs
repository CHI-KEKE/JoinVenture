using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeHost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Host",
                table: "Activities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "Activities",
                type: "TEXT",
                nullable: true);
        }
    }
}
