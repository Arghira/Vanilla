using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vanilla.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDeterministic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_TableId_StartAt",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TableId",
                table: "Reservations",
                column: "TableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_TableId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TableId_StartAt",
                table: "Reservations",
                columns: new[] { "TableId", "StartAt" },
                unique: true);
        }
    }
}
