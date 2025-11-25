using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biograf.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filmer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Beskrivning = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Langd = table.Column<int>(type: "int", nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filmer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salonger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Salongsnummer = table.Column<int>(type: "int", nullable: false),
                    AntalStolar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salonger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forestallningar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    SalongId = table.Column<int>(type: "int", nullable: false),
                    DatumTid = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forestallningar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forestallningar_Filmer_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Filmer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forestallningar_Salonger_SalongId",
                        column: x => x.SalongId,
                        principalTable: "Salonger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bokningar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForestallningId = table.Column<int>(type: "int", nullable: false),
                    Stolnummer = table.Column<int>(type: "int", nullable: false),
                    Bokningsnummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KundNamn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KundEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bokningar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bokningar_Forestallningar_ForestallningId",
                        column: x => x.ForestallningId,
                        principalTable: "Forestallningar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bokningar_Bokningsnummer",
                table: "Bokningar",
                column: "Bokningsnummer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bokningar_ForestallningId_Stolnummer",
                table: "Bokningar",
                columns: new[] { "ForestallningId", "Stolnummer" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forestallningar_FilmId",
                table: "Forestallningar",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Forestallningar_SalongId",
                table: "Forestallningar",
                column: "SalongId");

            migrationBuilder.CreateIndex(
                name: "IX_Salonger_Salongsnummer",
                table: "Salonger",
                column: "Salongsnummer",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bokningar");

            migrationBuilder.DropTable(
                name: "Forestallningar");

            migrationBuilder.DropTable(
                name: "Filmer");

            migrationBuilder.DropTable(
                name: "Salonger");
        }
    }
}
