using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheravexBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePDF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                table: "Factures",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FullNumber",
                table: "Factures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TypeDocument",
                table: "Factures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullNumber",
                table: "Factures");

            migrationBuilder.DropColumn(
                name: "TypeDocument",
                table: "Factures");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Factures",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
