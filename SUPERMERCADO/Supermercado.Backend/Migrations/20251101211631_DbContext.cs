using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermercado.Backend.Migrations
{
    /// <inheritdoc />
    public partial class DbContext : Migration
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
                name: "Rols",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    movimiento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_codigo_tipodoc = table.Column<int>(type: "int", nullable: false),
                    FK_consecutivo_id = table.Column<int>(type: "int", nullable: false),
                    numero_documento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FK_usuario_id = table.Column<int>(type: "int", nullable: false),
                    FK_tercero_id = table.Column<int>(type: "int", nullable: false),
                    observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.movimiento_id);
                    table.ForeignKey(
                        name: "FK_Movimientos_Terceros_FK_tercero_id",
                        column: x => x.FK_tercero_id,
                        principalTable: "Terceros",
                        principalColumn: "tercero_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimientos_Usuarios_FK_usuario_id",
                        column: x => x.FK_usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimientos_consecutivos_FK_consecutivo_id",
                        column: x => x.FK_consecutivo_id,
                        principalTable: "consecutivos",
                        principalColumn: "consecutivo_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimientos_tipoDctos_FK_codigo_tipodoc",
                        column: x => x.FK_codigo_tipodoc,
                        principalTable: "tipoDctos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    factura_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_movimiento_id = table.Column<int>(type: "int", nullable: false),
                    total_bruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_descuentos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_impu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_neto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.factura_id);
                    table.ForeignKey(
                        name: "FK_Facturas_Movimientos_FK_movimiento_id",
                        column: x => x.FK_movimiento_id,
                        principalTable: "Movimientos",
                        principalColumn: "movimiento_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesFactura",
                columns: table => new
                {
                    detalle_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_factura_id = table.Column<int>(type: "int", nullable: false),
                    FK_producto_id = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    descuento_porcentaje = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    descuento_valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesFactura", x => x.detalle_id);
                    table.ForeignKey(
                        name: "FK_DetallesFactura_Facturas_FK_factura_id",
                        column: x => x.FK_factura_id,
                        principalTable: "Facturas",
                        principalColumn: "factura_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesFactura_Productos_FK_producto_id",
                        column: x => x.FK_producto_id,
                        principalTable: "Productos",
                        principalColumn: "producto_id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Categoria_Productos_descripcion",
                table: "Categoria_Productos",
                column: "descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consecutivos_cod_consecut",
                table: "consecutivos",
                column: "cod_consecut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consecutivos_FK_codigo_tipodcto",
                table: "consecutivos",
                column: "FK_codigo_tipodcto");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFactura_detalle_id",
                table: "DetallesFactura",
                column: "detalle_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFactura_FK_factura_id",
                table: "DetallesFactura",
                column: "FK_factura_id");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFactura_FK_producto_id",
                table: "DetallesFactura",
                column: "FK_producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_factura_id",
                table: "Facturas",
                column: "factura_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_FK_movimiento_id",
                table: "Facturas",
                column: "FK_movimiento_id");

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
                name: "IX_Movimientos_FK_codigo_tipodoc",
                table: "Movimientos",
                column: "FK_codigo_tipodoc");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_FK_consecutivo_id",
                table: "Movimientos",
                column: "FK_consecutivo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_FK_tercero_id",
                table: "Movimientos",
                column: "FK_tercero_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_FK_usuario_id",
                table: "Movimientos",
                column: "FK_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_movimiento_id",
                table: "Movimientos",
                column: "movimiento_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_numero_documento",
                table: "Movimientos",
                column: "numero_documento",
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

            migrationBuilder.CreateIndex(
                name: "IX_tipoDctos_Codigo",
                table: "tipoDctos",
                column: "Codigo",
                unique: true);

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
                name: "DetallesFactura");

            migrationBuilder.DropTable(
                name: "PagosFactura");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "MetodosPago");

            migrationBuilder.DropTable(
                name: "Categoria_Productos");

            migrationBuilder.DropTable(
                name: "Tarifa_IVAs");

            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "Terceros");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "consecutivos");

            migrationBuilder.DropTable(
                name: "TiposIdentificacions");

            migrationBuilder.DropTable(
                name: "Rols");

            migrationBuilder.DropTable(
                name: "tipoDctos");
        }
    }
}
