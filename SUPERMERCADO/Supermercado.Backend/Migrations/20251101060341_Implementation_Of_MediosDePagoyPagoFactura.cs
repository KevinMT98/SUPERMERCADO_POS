using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermercado.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Implementation_Of_MediosDePagoyPagoFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetodosPago",
                columns: table => new
                {
                    id_metodo_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    metodo_pago = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    codigo_metpag = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodosPago", x => x.id_metodo_pago);
                });

            migrationBuilder.CreateTable(
                name: "PagosFactura",
                columns: table => new
                {
                    pago_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_factura_id = table.Column<int>(type: "int", nullable: false),
                    FK_id_metodo_pago = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    referencia_pago = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosFactura", x => x.pago_id);
                    table.ForeignKey(
                        name: "FK_PagosFactura_Facturas_FK_factura_id",
                        column: x => x.FK_factura_id,
                        principalTable: "Facturas",
                        principalColumn: "factura_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagosFactura_MetodosPago_FK_id_metodo_pago",
                        column: x => x.FK_id_metodo_pago,
                        principalTable: "MetodosPago",
                        principalColumn: "id_metodo_pago",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetodosPago_codigo_metpag",
                table: "MetodosPago",
                column: "codigo_metpag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetodosPago_id_metodo_pago",
                table: "MetodosPago",
                column: "id_metodo_pago",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagosFactura_FK_factura_id",
                table: "PagosFactura",
                column: "FK_factura_id");

            migrationBuilder.CreateIndex(
                name: "IX_PagosFactura_FK_id_metodo_pago",
                table: "PagosFactura",
                column: "FK_id_metodo_pago");

            migrationBuilder.CreateIndex(
                name: "IX_PagosFactura_pago_id",
                table: "PagosFactura",
                column: "pago_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagosFactura");

            migrationBuilder.DropTable(
                name: "MetodosPago");
        }
    }
}
