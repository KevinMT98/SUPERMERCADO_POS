using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermercado.Backend.Migrations
{
    /// <inheritdoc />
    public partial class entidad_Tercero_again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Terceros",
                columns: table => new
                {
                    tercero_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_codigo_ident = table.Column<int>(type: "int", nullable: false),
                    numero_identificacion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombre2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    apellido1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    es_proveedor = table.Column<bool>(type: "bit", nullable: false),
                    es_cliente = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terceros", x => x.tercero_id);
                    table.ForeignKey(
                        name: "FK_Terceros_TiposIdentificacions_FK_codigo_ident",
                        column: x => x.FK_codigo_ident,
                        principalTable: "TiposIdentificacions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Terceros_FK_codigo_ident",
                table: "Terceros",
                column: "FK_codigo_ident");

            migrationBuilder.CreateIndex(
                name: "IX_Terceros_numero_identificacion",
                table: "Terceros",
                column: "numero_identificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terceros_tercero_id",
                table: "Terceros",
                column: "tercero_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Terceros");
        }
    }
}
