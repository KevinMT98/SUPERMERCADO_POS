using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermercado.Backend.Migrations
{
    /// <inheritdoc />
    public partial class initialDb_Completo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria_Productos",
                columns: table => new
                {
                    categoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria_Productos", x => x.categoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Rols",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rols", x => x.rol_id);
                });

            migrationBuilder.CreateTable(
                name: "Tarifa_IVAs",
                columns: table => new
                {
                    tarifa_iva_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo_Iva = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    porcentaje = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifa_IVAs", x => x.tarifa_iva_id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_usuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FK_rol_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.usuario_id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Rols_FK_rol_id",
                        column: x => x.FK_rol_id,
                        principalTable: "Rols",
                        principalColumn: "rol_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    producto_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo_producto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    codigo_barras = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    precio_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FK_categoria_id = table.Column<int>(type: "int", nullable: false),
                    FK_codigo_iva = table.Column<int>(type: "int", nullable: false),
                    stock_actual = table.Column<int>(type: "int", nullable: false),
                    stock_minimo = table.Column<int>(type: "int", nullable: false),
                    stock_maximo = table.Column<int>(type: "int", nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.producto_id);
                    table.ForeignKey(
                        name: "FK_Productos_Categoria_Productos_FK_categoria_id",
                        column: x => x.FK_categoria_id,
                        principalTable: "Categoria_Productos",
                        principalColumn: "categoriaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productos_Tarifa_IVAs_FK_codigo_iva",
                        column: x => x.FK_codigo_iva,
                        principalTable: "Tarifa_IVAs",
                        principalColumn: "tarifa_iva_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Productos_descripcion",
                table: "Categoria_Productos",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_codigo_barras",
                table: "Productos",
                column: "codigo_barras",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_codigo_producto",
                table: "Productos",
                column: "codigo_producto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_FK_categoria_id",
                table: "Productos",
                column: "FK_categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_FK_codigo_iva",
                table: "Productos",
                column: "FK_codigo_iva");

            migrationBuilder.CreateIndex(
                name: "IX_Rols_nombre",
                table: "Rols",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tarifa_IVAs_codigo_Iva",
                table: "Tarifa_IVAs",
                column: "codigo_Iva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tarifa_IVAs_tarifa_iva_id",
                table: "Tarifa_IVAs",
                column: "tarifa_iva_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_email",
                table: "Usuarios",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FK_rol_id",
                table: "Usuarios",
                column: "FK_rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_nombre_usuario",
                table: "Usuarios",
                column: "nombre_usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categoria_Productos");

            migrationBuilder.DropTable(
                name: "Tarifa_IVAs");

            migrationBuilder.DropTable(
                name: "Rols");
        }
    }
}
