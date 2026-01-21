using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheravexBackend.Migrations
{
    /// <inheritdoc />
    public partial class FournisseurLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FournisseurId",
                table: "Lot",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lot_FournisseurId",
                table: "Lot",
                column: "FournisseurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lot_Fournisseur_FournisseurId",
                table: "Lot",
                column: "FournisseurId",
                principalTable: "Fournisseur",
                principalColumn: "FournisseurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lot_Fournisseur_FournisseurId",
                table: "Lot");

            migrationBuilder.DropIndex(
                name: "IX_Lot_FournisseurId",
                table: "Lot");

            migrationBuilder.DropColumn(
                name: "FournisseurId",
                table: "Lot");
        }
    }
}
