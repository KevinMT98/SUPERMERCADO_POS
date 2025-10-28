using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermercado.Backend.Migrations
{
    /// <inheritdoc />
    public partial class entidad_TipoDctos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tipoDctos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipoDctos", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tipoDctos_Codigo",
                table: "tipoDctos",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tipoDctos");
        }
    }
}
