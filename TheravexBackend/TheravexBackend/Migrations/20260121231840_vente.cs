using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheravexBackend.Migrations
{
    /// <inheritdoc />
    public partial class vente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vente",
                columns: table => new
                {
                    VenteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vente", x => x.VenteId);
                });

            migrationBuilder.CreateTable(
                name: "LigneVente",
                columns: table => new
                {
                    LigneVenteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    SellQuantity = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LotId = table.Column<int>(type: "int", nullable: false),
                    VenteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LigneVente", x => x.LigneVenteId);
                    table.ForeignKey(
                        name: "FK_LigneVente_Vente_VenteId",
                        column: x => x.VenteId,
                        principalTable: "Vente",
                        principalColumn: "VenteId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LigneVente_VenteId",
                table: "LigneVente",
                column: "VenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LigneVente");

            migrationBuilder.DropTable(
                name: "Vente");
        }
    }
}
