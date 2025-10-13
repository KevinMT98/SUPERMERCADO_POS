using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermercado.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ModRol_activo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "activo",
                table: "Rols",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activo",
                table: "Rols");
        }
    }
}
