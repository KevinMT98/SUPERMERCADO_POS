using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermercado.Backend.Migrations
{
    /// <inheritdoc />
    public partial class entidad_Consecutivos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "consecutivos",
                columns: table => new
                {
                    consecutivo_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cod_consecut = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_codigo_tipodcto = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    consecutivo_ini = table.Column<int>(type: "int", nullable: false),
                    consecutivo_fin = table.Column<int>(type: "int", nullable: false),
                    consecutivo_actual = table.Column<int>(type: "int", nullable: false),
                    afecta_inv = table.Column<bool>(type: "bit", nullable: false),
                    es_entrada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consecutivos", x => x.consecutivo_Id);
                    table.ForeignKey(
                        name: "FK_consecutivos_tipoDctos_FK_codigo_tipodcto",
                        column: x => x.FK_codigo_tipodcto,
                        principalTable: "tipoDctos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_consecutivos_cod_consecut",
                table: "consecutivos",
                column: "cod_consecut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consecutivos_FK_codigo_tipodcto",
                table: "consecutivos",
                column: "FK_codigo_tipodcto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "consecutivos");
        }
    }
}
