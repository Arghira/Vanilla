using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vanilla.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TableId = table.Column<short>(type: "INTEGER", nullable: false),
                    StartAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    PartySize = table.Column<int>(type: "INTEGER", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Masa 1" },
                    { (short)2, "Masa 2" },
                    { (short)3, "Masa 3" },
                    { (short)4, "Masa 4" },
                    { (short)5, "Masa 5" },
                    { (short)6, "Masa 6" },
                    { (short)7, "Masa 7" },
                    { (short)8, "Masa 8" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TableId_StartAt",
                table: "Reservations",
                columns: new[] { "TableId", "StartAt" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
