using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermercado.Backend.Migrations
{
    /// <inheritdoc />
    public partial class entidad_tipod_identificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposIdentificacions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipoDocumentoID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposIdentificacions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TiposIdentificacions_ID",
                table: "TiposIdentificacions",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposIdentificacions_tipoDocumentoID",
                table: "TiposIdentificacions",
                column: "tipoDocumentoID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TiposIdentificacions");
        }
    }
}
