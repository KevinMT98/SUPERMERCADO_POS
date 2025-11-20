/**
 * SUPERMERCADO POS - Facturación
 * Lógica principal para el módulo de facturación
 */

// Variables globales
let productos = [];
let clientes = [];
let metodosPago = [];
let productosFactura = [];
let pagosFactura = [];
let debounceTimer = null;

// Mapeo de códigos IVA a porcentajes (basado en la estructura del backend)
const CODIGOS_IVA = {
    1: 0,   // Exento
    2: 5,   // IVA reducido
    3: 19,  // IVA general
    4: 0    // Sin IVA
};

/**
 * Obtener porcentaje de IVA desde el producto
 */
function obtenerPorcentajeIva(producto) {
    // Intentar obtener el porcentaje directamente
    let iva = parseFloat(producto.porcentajeIva || producto.porcentaje_iva || producto.iva) || 0;
    
    // Si no hay porcentaje directo, usar el código IVA
    if (iva === 0 && producto.codigoIva) {
        iva = CODIGOS_IVA[producto.codigoIva] || 0;
    }
    
    return iva;
}

// Proteger la página
authService.requireAuth();

// Mostrar email del usuario
const user = authService.getCurrentUser();
if (user) {
    document.getElementById('userEmail').textContent = user.email;
}

// Inicializar
document.addEventListener('DOMContentLoaded', async () => {
    await inicializarDatos();
    configurarEventos();
    
    // Intentar recuperar estado temporal después de cargar datos
    const recuperado = recuperarEstadoTemporal();
    if (!recuperado) {
        actualizarTotales();
    }
    
    configurarProteccionNavegacion();
    inicializarTooltips();
});

/**
 * Configurar protección contra navegación accidental
 */
function configurarProteccionNavegacion() {
    // Advertir antes de salir si hay productos en la factura
    window.addEventListener('beforeunload', (e) => {
        if (productosFactura.length > 0) {
            e.preventDefault();
            e.returnValue = 'Hay productos en la factura. ¿Está seguro de que desea salir?';
            return e.returnValue;
        }
    });
    
    // Interceptar clics en enlaces del navbar para confirmar
    document.querySelectorAll('.navbar a[href]').forEach(link => {
        if (link.getAttribute('href') !== '#' && !link.getAttribute('href').includes('javascript:')) {
            link.addEventListener('click', (e) => {
                if (productosFactura.length > 0) {
                    const confirmacion = confirm('Hay productos en la factura. ¿Está seguro de que desea navegar a otra página? Se perderán los datos no guardados.');
                    if (!confirmacion) {
                        e.preventDefault();
                        return false;
                    }
                }
            });
        }
    });
}

/**
 * Inicializar tooltips de Bootstrap
 */
function inicializarTooltips() {
    // Inicializar todos los tooltips en la página
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    const tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
    
    console.log('Tooltips inicializados:', tooltipList.length);
}

/**
 * Inicializar datos necesarios
 */
async function inicializarDatos() {
    try {
        mostrarLoading(true);
        
        // Cargar datos en paralelo
        const [productosResponse, clientesResponse, metodosResponse] = await Promise.all([
            facturacionService.obtenerProductosDisponibles(),
            facturacionService.obtenerClientes(),
            facturacionService.obtenerMetodosPago()
        ]);

        productos = productosResponse || [];
        clientes = clientesResponse || [];
        metodosPago = metodosResponse || [];

        console.log('Datos cargados:', { productos: productos.length, clientes: clientes.length, metodosPago: metodosPago.length });
        
        // Debug: mostrar algunos productos de ejemplo
        if (productos.length > 0) {
            console.log('Ejemplo de productos cargados:', productos.slice(0, 3));
            console.log('Estructura del primer producto:', productos[0]);
            console.log('Propiedades detectadas:', {
                id: productos[0].productoId || productos[0].producto_id || productos[0].id,
                codigo: productos[0].codigoProducto || productos[0].codigo_producto || productos[0].codigo,
                nombre: productos[0].nombre || productos[0].nombreProducto || productos[0].descripcion,
                precio: productos[0].precioUnitario || productos[0].precio_venta || productos[0].precio,
                stock: productos[0].stockActual || productos[0].stock_actual || productos[0].stock,
                iva: obtenerPorcentajeIva(productos[0])
            });
            console.log('Precio formateado:', facturacionService.formatearMoneda(productos[0].precioUnitario || productos[0].precio_venta || productos[0].precio || 0));
            console.log('Propiedades IVA disponibles:', {
                porcentajeIva: productos[0].porcentajeIva,
                porcentaje_iva: productos[0].porcentaje_iva,
                iva: productos[0].iva,
                codigoIva: productos[0].codigoIva,
                iva_calculado: obtenerPorcentajeIva(productos[0]),
                todas_propiedades: Object.keys(productos[0])
            });
            
            // Verificar algunos productos más para el IVA
            productos.slice(0, 3).forEach((prod, idx) => {
                console.log(`Producto ${idx + 1} - ${prod.codigoProducto}: IVA = ${obtenerPorcentajeIva(prod)}% (codigoIva: ${prod.codigoIva})`);
            });
        } else {
            console.warn('⚠️ No se cargaron productos. Verificar conexión con el backend.');
            Helpers.showToast('No se pudieron cargar los productos. Verifique la conexión.', 'warning');
        }

        // Poblar selects
        poblarSelectClientes();
        poblarSelectMetodosPago();

    } catch (error) {
        console.error('Error al inicializar datos:', error);
        Helpers.showToast('Error al cargar datos iniciales: ' + error.message, 'error');
    } finally {
        mostrarLoading(false);
    }
}

/**
 * Configurar eventos
 */
function configurarEventos() {
    // Búsqueda de productos con debounce
    document.getElementById('buscarProducto').addEventListener('input', (e) => {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => {
            buscarProductos(e.target.value);
        }, 300);
    });

    // Ocultar resultados al hacer clic fuera
    document.addEventListener('click', (e) => {
        if (!e.target.closest('.search-container')) {
            document.getElementById('searchResults').style.display = 'none';
        }
    });

    // Formulario principal
    document.getElementById('formFacturacion').addEventListener('submit', guardarFactura);

    // Eventos de teclado para búsqueda
    document.getElementById('buscarProducto').addEventListener('keydown', (e) => {
        if (e.key === 'Enter') {
            e.preventDefault();
            const firstResult = document.querySelector('.search-result-item');
            if (firstResult) {
                firstResult.click();
            }
        }
    });
}

/**
 * Poblar select de clientes
 */
function poblarSelectClientes() {
    const select = document.getElementById('clienteSelect');
    select.innerHTML = '<option value="">Seleccione un cliente...</option>';
    
    clientes.forEach(cliente => {
        const nombreCompleto = `${cliente.nombre} ${cliente.apellido1 || ''}`.trim();
        const option = document.createElement('option');
        option.value = cliente.tercero_id || cliente.terceroId;
        option.textContent = `${cliente.numero_identificacion} - ${nombreCompleto}`;
        select.appendChild(option);
    });
}

/**
 * Poblar select de métodos de pago
 */
function poblarSelectMetodosPago() {
    const select = document.getElementById('metodoPagoSelect');
    select.innerHTML = '<option value="">Seleccione...</option>';
    
    metodosPago.forEach(metodo => {
        const option = document.createElement('option');
        option.value = metodo.id_metodo_pago || metodo.metodoPagoId;
        option.textContent = metodo.metodo_pago || metodo.nombre;
        select.appendChild(option);
    });
}

/**
 * Buscar productos
 */
function buscarProductos(termino) {
    const resultsContainer = document.getElementById('searchResults');
    
    console.log('Buscando productos con término:', termino);
    console.log('Total productos disponibles:', productos.length);
    
    if (!termino || termino.length < 2) {
        resultsContainer.style.display = 'none';
        return;
    }

    if (productos.length === 0) {
        resultsContainer.innerHTML = '<div class="search-result-item text-warning">⚠️ No hay productos cargados. <a href="#" onclick="mostrarTodosLosProductos()">Ver todos</a></div>';
        resultsContainer.style.display = 'block';
        return;
    }

    const resultados = productos.filter(producto => {
        const codigo = producto.codigoProducto || producto.codigo_producto || producto.codigo || '';
        const nombre = producto.nombre || producto.nombreProducto || producto.descripcion || '';
        
        return codigo.toLowerCase().includes(termino.toLowerCase()) ||
               nombre.toLowerCase().includes(termino.toLowerCase());
    }).slice(0, 10); // Limitar a 10 resultados

    console.log('Resultados encontrados:', resultados.length);

    if (resultados.length === 0) {
        resultsContainer.innerHTML = `
            <div class="search-result-item text-muted">
                No se encontraron productos con "${termino}"
                <br>
                <small><a href="#" onclick="mostrarTodosLosProductos()">Ver todos los productos disponibles</a></small>
            </div>
        `;
    } else {
        resultsContainer.innerHTML = resultados.map(producto => {
            const id = producto.productoId || producto.producto_id || producto.id;
            const codigo = producto.codigoProducto || producto.codigo_producto || producto.codigo;
            const nombre = producto.nombre || producto.nombreProducto || producto.descripcion;
            const precio = producto.precioUnitario || producto.precio_venta || producto.precio;
            const stock = producto.stockActual || producto.stock_actual || producto.stock;
            const iva = obtenerPorcentajeIva(producto);
            
            return `
                <div class="search-result-item" onclick="agregarProductoDesdeResultado(${id})" 
                     style="cursor: pointer; padding: 0.75rem; border-bottom: 1px solid #f1f3f4; transition: all 0.2s;">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div class="fw-bold text-primary">${codigo}</div>
                            <div class="text-dark">${nombre}</div>
                            <small class="text-muted">
                                <i class="bi bi-box"></i> Stock: <span class="text-success fw-bold">${stock}</span> | 
                                <i class="bi bi-currency-dollar"></i> Precio: <span class="text-primary fw-bold">${facturacionService.formatearMoneda(precio)}</span>
                                ${iva > 0 ? ` | <i class="bi bi-percent"></i> IVA: ${iva}%` : ''}
                            </small>
                        </div>
                        <div class="text-end">
                            <button class="btn btn-outline-primary btn-sm" onclick="event.stopPropagation(); agregarProductoDesdeResultado(${id})">
                                <i class="bi bi-plus"></i> Agregar
                            </button>
                        </div>
                    </div>
                </div>
            `;
        }).join('');
    }

    resultsContainer.style.display = 'block';
}

/**
 * Agregar producto desde resultado de búsqueda
 */
function agregarProductoDesdeResultado(productoId) {
    document.getElementById('searchResults').style.display = 'none';
    document.getElementById('buscarProducto').value = '';
    agregarProducto(productoId);
}

/**
 * Agregar producto a la factura
 */
function agregarProducto(productoId) {
    console.log('Intentando agregar producto ID:', productoId);
    console.log('Productos disponibles:', productos.length);
    console.log('Productos en factura antes:', productosFactura.length);
    
    // Buscar producto con diferentes formatos de ID
    const producto = productos.find(p => 
        p.producto_id === productoId || 
        p.productoId === productoId || 
        p.id === productoId
    );
    
    if (!producto) {
        console.error('Producto no encontrado con ID:', productoId);
        console.error('IDs disponibles:', productos.map(p => ({
            producto_id: p.producto_id,
            productoId: p.productoId,
            id: p.id
        })));
        Helpers.showToast('Producto no encontrado', 'error');
        return;
    }
    
    console.log('Producto encontrado:', producto);

    // Verificar si ya está en la factura usando diferentes formatos de ID
    const yaEnFactura = productosFactura.find(p => 
        p.producto_id === productoId || 
        p.productoId === productoId || 
        p.id === productoId
    );
    
    if (yaEnFactura) {
        Helpers.showToast('El producto ya está en la factura', 'warning');
        return;
    }

    // Extraer propiedades con diferentes formatos
    const id = producto.productoId || producto.producto_id || producto.id;
    const codigo = producto.codigoProducto || producto.codigo_producto || producto.codigo;
    const nombre = producto.nombre || producto.nombreProducto || producto.descripcion;
    const precio = parseFloat(producto.precioUnitario || producto.precio_venta || producto.precio) || 0;
    const stock = parseInt(producto.stockActual || producto.stock_actual || producto.stock) || 0;
    const iva = obtenerPorcentajeIva(producto);

    // Agregar producto con valores inicializados desde el maestro
    const productoFactura = {
        producto_id: id,
        codigo_producto: codigo,
        nombre: nombre,
        precio_unitario: precio, // Precio del maestro de productos
        cantidad: 1,
        descuento_porcentaje: 0,
        descuento_valor: 0,
        porcentaje_iva: iva,
        stock_disponible: stock
    };
    
    console.log('Producto agregado con precio del maestro:', {
        id: id,
        codigo: codigo,
        nombre: nombre,
        precio_maestro: precio,
        stock: stock,
        iva: iva
    });
    
    console.log('Objeto producto en factura:', productoFactura);

    productosFactura.push(productoFactura);
    
    console.log('Producto agregado. Total productos en factura:', productosFactura.length);
    
    // Guardar estado temporal
    guardarEstadoTemporal();
    
    // Limpiar búsqueda
    document.getElementById('buscarProducto').value = '';
    document.getElementById('searchResults').style.display = 'none';
    
    // Actualizar UI
    renderizarProductos();
    actualizarTotales();
    
    Helpers.showToast(`Producto "${producto.nombre}" agregado`, 'success');
}

/**
 * Renderizar productos en la factura
 */
function renderizarProductos() {
    const container = document.getElementById('productosContainer');
    const contador = document.getElementById('contadorProductos');
    
    // Debug: verificar estado de productos
    console.log('Renderizando productos. Total:', productosFactura.length, productosFactura);
    
    contador.textContent = `${productosFactura.length} producto${productosFactura.length !== 1 ? 's' : ''}`;

    if (productosFactura.length === 0) {
        container.innerHTML = `
            <div class="text-center text-muted py-4">
                <i class="bi bi-cart-x fs-1"></i>
                <p class="mt-2">No hay productos agregados</p>
                <small>Use el buscador para agregar productos a la factura</small>
            </div>
        `;
        return;
    }

    container.innerHTML = productosFactura.map((producto, index) => {
        // Cálculo del total por línea: (cantidad * precio_unitario) - descuento + IVA
        const subtotalBruto = producto.cantidad * producto.precio_unitario;
        const descuentoValor = producto.descuento_valor || (subtotalBruto * (producto.descuento_porcentaje / 100));
        const baseGravable = subtotalBruto - descuentoValor;
        const valorIva = baseGravable * (producto.porcentaje_iva / 100);
        const totalLinea = baseGravable + valorIva;
        
        // Debug: verificar cálculo de línea
        if (index === 0) {
            console.log(`Ejemplo cálculo línea - ${producto.codigo_producto}:`, {
                subtotalBruto, descuentoValor, baseGravable, 
                porcentajeIva: producto.porcentaje_iva, valorIva, totalLinea
            });
        }
        
        return `
            <div class="card mb-3 shadow-sm animate-slide-up producto-card">
                <div class="card-header bg-light py-2">
                    <div class="row align-items-center">
                        <div class="col">
                            <div class="d-flex align-items-center">
                                <div class="product-icon me-3">
                                    <i class="bi bi-box-seam fs-4 text-primary"></i>
                                </div>
                                <div>
                                    <h6 class="mb-0 fw-bold text-primary">${producto.codigo_producto}</h6>
                                    <small class="text-muted">${producto.nombre}</small>
                                </div>
                            </div>
                        </div>
                        <div class="col-auto">
                            <div class="d-flex align-items-center gap-2">
                                <span class="badge bg-success">
                                    <i class="bi bi-box"></i> Stock: ${producto.stock_disponible}
                                </span>
                                ${producto.porcentaje_iva > 0 ? `
                                    <span class="badge bg-info">
                                        <i class="bi bi-percent"></i> IVA: ${producto.porcentaje_iva}%
                                    </span>
                                ` : ''}
                                <button class="btn btn-outline-danger btn-sm" onclick="eliminarProducto(${index})" 
                                        title="Eliminar producto" data-bs-toggle="tooltip">
                                    <i class="bi bi-trash"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body py-3">
                    <div class="row g-3 align-items-end">
                        <!-- Cantidad -->
                        <div class="col-md-2">
                            <label class="form-label fw-bold text-secondary mb-1">Cantidad</label>
                            <input type="number" class="form-control text-center fw-bold" 
                                   value="${producto.cantidad}" min="1" max="${producto.stock_disponible}"
                                   onchange="actualizarCantidadProducto(${index}, this.value)">
                        </div>
                        
                        <!-- Precio Unitario -->
                        <div class="col-md-2">
                            <label class="form-label fw-bold text-secondary mb-1">Precio Unitario</label>
                            <div class="input-group">
                                <span class="input-group-text">$</span>
                                <input type="number" class="form-control fw-bold" 
                                       value="${producto.precio_unitario}" step="0.01" min="0"
                                       onchange="actualizarPrecioProducto(${index}, this.value)">
                            </div>
                        </div>
                        
                        <!-- Descuento % -->
                        <div class="col-md-2">
                            <label class="form-label fw-bold text-secondary mb-1">Descuento %</label>
                            <div class="input-group">
                                <input type="number" class="form-control fw-bold" 
                                       value="${producto.descuento_porcentaje}" step="0.01" min="0" max="100"
                                       onchange="actualizarDescuentoPorcentaje(${index}, this.value)"
                                       placeholder="0">
                                <span class="input-group-text">%</span>
                            </div>
                        </div>
                        
                        <!-- IVA % -->
                        <div class="col-md-2">
                            <label class="form-label fw-bold text-secondary mb-1">IVA %</label>
                            <div class="input-group">
                                <input type="number" class="form-control fw-bold bg-light" 
                                       value="${producto.porcentaje_iva}" step="0.01" min="0"
                                       readonly title="IVA del maestro de productos">
                                <span class="input-group-text">%</span>
                            </div>
                        </div>
                        
                        <!-- Total por Línea -->
                        <div class="col-md-2">
                            <label class="form-label fw-bold text-secondary mb-1">Total Línea</label>
                            <div class="input-group">
                                <span class="input-group-text">$</span>
                                <input type="text" class="form-control fw-bold text-success bg-light text-end" 
                                       value="${facturacionService.formatearMoneda(totalLinea).replace('$', '')}" 
                                       readonly title="${producto.cantidad} × ${facturacionService.formatearMoneda(producto.precio_unitario)}${descuentoValor > 0 ? ` - ${facturacionService.formatearMoneda(descuentoValor)}` : ''} + IVA ${producto.porcentaje_iva}% (${facturacionService.formatearMoneda(valorIva)}) = ${facturacionService.formatearMoneda(totalLinea)}">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }).join('');
}

/**
 * Actualizar cantidad de producto
 */
function actualizarCantidadProducto(index, cantidad) {
    cantidad = parseInt(cantidad);
    const producto = productosFactura[index];
    
    if (cantidad <= 0 || cantidad > producto.stock_disponible) {
        Helpers.showToast(`Cantidad inválida. Stock disponible: ${producto.stock_disponible}`, 'error');
        renderizarProductos();
        return;
    }
    
    const cantidadAnterior = producto.cantidad;
    producto.cantidad = cantidad;
    
    // Calcular nuevo total de línea incluyendo IVA
    const subtotalBruto = cantidad * producto.precio_unitario;
    const descuentoValor = producto.descuento_valor || (subtotalBruto * (producto.descuento_porcentaje / 100));
    const baseGravable = subtotalBruto - descuentoValor;
    const valorIva = baseGravable * (producto.porcentaje_iva / 100);
    const totalLinea = baseGravable + valorIva;
    
    console.log(`Cantidad actualizada para ${producto.nombre}: ${cantidadAnterior} → ${cantidad}. Total línea: ${facturacionService.formatearMoneda(totalLinea)}`);
    
    guardarEstadoTemporal();
    renderizarProductos();
    actualizarTotales();
    
    // Mostrar feedback visual
    Helpers.showToast(`Cantidad actualizada: ${producto.nombre} (${cantidad} unidades)`, 'success');
}

/**
 * Actualizar precio de producto
 */
function actualizarPrecioProducto(index, precio) {
    precio = parseFloat(precio);
    if (precio <= 0) {
        Helpers.showToast('Precio inválido', 'error');
        renderizarProductos();
        return;
    }
    
    productosFactura[index].precio_unitario = precio;
    guardarEstadoTemporal();
    renderizarProductos();
    actualizarTotales();
}

/**
 * Actualizar descuento por porcentaje
 */
function actualizarDescuentoPorcentaje(index, porcentaje) {
    porcentaje = parseFloat(porcentaje) || 0;
    if (porcentaje < 0 || porcentaje > 100) {
        Helpers.showToast('Descuento inválido (0-100%)', 'error');
        renderizarProductos();
        return;
    }
    
    productosFactura[index].descuento_porcentaje = porcentaje;
    productosFactura[index].descuento_valor = 0; // Siempre usar porcentaje, no valor directo
    
    console.log(`Descuento por porcentaje actualizado: ${porcentaje}% para producto ${productosFactura[index].nombre}`);
    
    guardarEstadoTemporal();
    renderizarProductos();
    actualizarTotales();
}

/**
 * Eliminar producto de la factura
 */
function eliminarProducto(index) {
    const producto = productosFactura[index];
    
    // Confirmar eliminación
    const confirmar = confirm(`¿Está seguro de eliminar "${producto.nombre}" de la factura?`);
    if (!confirmar) return;
    
    productosFactura.splice(index, 1);
    guardarEstadoTemporal();
    renderizarProductos();
    actualizarTotales();
    Helpers.showToast(`Producto "${producto.nombre}" eliminado`, 'info');
}

/**
 * Duplicar producto en la factura
 */
function duplicarProducto(index) {
    const productoOriginal = productosFactura[index];
    
    // Crear una copia del producto
    const productoDuplicado = {
        ...productoOriginal,
        cantidad: 1, // Resetear cantidad a 1
        descuento_porcentaje: 0, // Resetear descuento
        descuento_valor: 0
    };
    
    // Verificar stock disponible
    const stockUsado = productosFactura
        .filter(p => p.producto_id === productoOriginal.producto_id)
        .reduce((total, p) => total + p.cantidad, 0);
    
    if (stockUsado >= productoOriginal.stock_disponible) {
        Helpers.showToast('No hay suficiente stock para duplicar este producto', 'warning');
        return;
    }
    
    // Agregar el producto duplicado
    productosFactura.push(productoDuplicado);
    guardarEstadoTemporal();
    renderizarProductos();
    actualizarTotales();
    
    Helpers.showToast(`Producto "${productoOriginal.nombre}" duplicado`, 'success');
}

/**
 * Actualizar totales de la factura
 */
function actualizarTotales() {
    const detalles = productosFactura.map(producto => ({
        cantidad: producto.cantidad,
        precioUnitario: producto.precio_unitario,
        descuentoPorcentaje: producto.descuento_porcentaje,
        descuentoValor: producto.descuento_valor,
        porcentajeIva: producto.porcentaje_iva || 0
    }));
    
    const totales = facturacionService.calcularTotalesFactura(detalles);
    const totalPagos = pagosFactura.reduce((sum, pago) => sum + pago.monto, 0);
    const diferencia = totalPagos - totales.totalNeto;

    // Actualizar UI
    document.getElementById('subtotalFactura').textContent = facturacionService.formatearMoneda(totales.totalBruto);
    document.getElementById('descuentosFactura').textContent = facturacionService.formatearMoneda(totales.totalDescuentos);
    document.getElementById('ivaFactura').textContent = facturacionService.formatearMoneda(totales.totalImpuestos);
    document.getElementById('totalFactura').textContent = facturacionService.formatearMoneda(totales.totalNeto);
    document.getElementById('totalPagos').textContent = facturacionService.formatearMoneda(totalPagos);
    document.getElementById('diferenciaPagos').textContent = facturacionService.formatearMoneda(Math.abs(diferencia));

    // Actualizar estado
    actualizarEstadoFactura(totales, totalPagos);
}

/**
 * Actualizar estado de la factura
 */
function actualizarEstadoFactura(totales, totalPagos) {
    const estadoDiv = document.getElementById('estadoFactura');
    const diferencia = totalPagos - totales.totalNeto;
    
    if (productosFactura.length === 0) {
        estadoDiv.className = 'alert alert-info';
        estadoDiv.innerHTML = '<i class="bi bi-info-circle"></i> Agregue productos para continuar';
    } else if (!document.getElementById('clienteSelect').value) {
        estadoDiv.className = 'alert alert-warning';
        estadoDiv.innerHTML = '<i class="bi bi-exclamation-triangle"></i> Seleccione un cliente';
    } else if (totales.totalNeto < 1000) {
        estadoDiv.className = 'alert alert-warning';
        estadoDiv.innerHTML = '<i class="bi bi-exclamation-triangle"></i> Monto mínimo: $1,000';
    } else if (Math.abs(diferencia) > 0.01) {
        if (diferencia < 0) {
            estadoDiv.className = 'alert alert-danger';
            estadoDiv.innerHTML = `<i class="bi bi-exclamation-circle"></i> Faltan ${facturacionService.formatearMoneda(Math.abs(diferencia))} en pagos`;
        } else {
            estadoDiv.className = 'alert alert-info';
            estadoDiv.innerHTML = `<i class="bi bi-info-circle"></i> Cambio: ${facturacionService.formatearMoneda(diferencia)}`;
        }
    } else {
        estadoDiv.className = 'alert alert-success';
        estadoDiv.innerHTML = '<i class="bi bi-check-circle"></i> Factura lista para guardar';
    }
}

/**
 * Agregar método de pago
 */
function agregarMetodoPago() {
    const modal = new bootstrap.Modal(document.getElementById('metodoPagoModal'));
    document.getElementById('formMetodoPago').reset();
    
    // Sugerir monto pendiente
    const totales = facturacionService.calcularTotalesFactura(productosFactura.map(p => ({
        cantidad: p.cantidad,
        precioUnitario: p.precio_unitario,
        descuentoPorcentaje: p.descuento_porcentaje,
        descuentoValor: p.descuento_valor,
        porcentajeIva: p.porcentaje_iva
    })));
    
    const totalPagos = pagosFactura.reduce((sum, pago) => sum + pago.monto, 0);
    const montoPendiente = totales.totalNeto - totalPagos;
    
    if (montoPendiente > 0) {
        document.getElementById('montoPago').value = montoPendiente.toFixed(2);
    }
    
    modal.show();
}

/**
 * Confirmar método de pago
 */
function confirmarMetodoPago() {
    const metodoPagoId = document.getElementById('metodoPagoSelect').value;
    const monto = parseFloat(document.getElementById('montoPago').value);
    const referencia = document.getElementById('referenciaPago').value;

    if (!metodoPagoId || !monto || monto <= 0) {
        Helpers.showToast('Complete todos los campos requeridos', 'error');
        return;
    }

    const metodoPago = metodosPago.find(m => (m.id_metodo_pago || m.metodoPagoId) == metodoPagoId);
    
    const pago = {
        metodoPagoId: parseInt(metodoPagoId),
        nombreMetodo: metodoPago?.metodo_pago || metodoPago?.nombre,
        monto: monto,
        referenciaPago: referencia
    };

    pagosFactura.push(pago);
    renderizarMetodosPago();
    actualizarTotales();

    const modal = bootstrap.Modal.getInstance(document.getElementById('metodoPagoModal'));
    modal.hide();

    Helpers.showToast('Método de pago agregado', 'success');
}

/**
 * Renderizar métodos de pago
 */
function renderizarMetodosPago() {
    const container = document.getElementById('metodsPagoContainer');

    if (pagosFactura.length === 0) {
        container.innerHTML = `
            <div class="text-center text-muted py-3">
                <i class="bi bi-credit-card fs-1"></i>
                <p class="mt-2">No hay métodos de pago agregados</p>
            </div>
        `;
        return;
    }

    container.innerHTML = pagosFactura.map((pago, index) => `
        <div class="metodo-pago-item animate-fade-in">
            <div class="row align-items-center">
                <div class="col-md-4">
                    <strong>${pago.nombreMetodo}</strong>
                </div>
                <div class="col-md-3">
                    <span class="fw-bold text-success">
                        ${facturacionService.formatearMoneda(pago.monto)}
                    </span>
                </div>
                <div class="col-md-4">
                    <small class="text-muted">${pago.referenciaPago || 'Sin referencia'}</small>
                </div>
                <div class="col-md-1">
                    <button type="button" class="btn btn-outline-danger btn-sm" 
                            onclick="eliminarMetodoPago(${index})" title="Eliminar">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        </div>
    `).join('');
}

/**
 * Eliminar método de pago
 */
function eliminarMetodoPago(index) {
    const pago = pagosFactura[index];
    pagosFactura.splice(index, 1);
    renderizarMetodosPago();
    actualizarTotales();
    Helpers.showToast(`Método de pago "${pago.nombreMetodo}" eliminado`, 'info');
}

/**
 * Guardar factura
 */
async function guardarFactura(e) {
    e.preventDefault();

    try {
        // Preparar datos para validación
        const datosValidacion = {
            terceroId: document.getElementById('clienteSelect').value,
            detalles: productosFactura.map(p => ({
                productoId: p.producto_id,
                cantidad: p.cantidad,
                precioUnitario: p.precio_unitario,
                descuentoPorcentaje: p.descuento_porcentaje,
                descuentoValor: p.descuento_valor,
                porcentajeIva: p.porcentaje_iva || 0
            })),
            pagos: pagosFactura
        };
        
        console.log('Datos para validación:', datosValidacion);
        console.log('Productos con IVA:', datosValidacion.detalles);
        
        // Validaciones locales
        const validacion = facturacionService.validarFacturaLocal(datosValidacion);

        if (!validacion.esValida) {
            console.error('Errores de validación:', validacion.errores);
            Helpers.showToast(validacion.errores[0], 'error');
            return;
        }

        mostrarLoading(true);

        // Generar objeto de factura
        const facturaData = facturacionService.generarObjetoFactura({
            terceroId: document.getElementById('clienteSelect').value,
            observaciones: document.getElementById('observaciones').value,
            detalles: productosFactura.map(p => ({
                productoId: p.producto_id,
                cantidad: p.cantidad,
                precioUnitario: p.precio_unitario,
                descuentoPorcentaje: p.descuento_porcentaje,
                descuentoValor: p.descuento_valor,
                porcentajeIva: p.porcentaje_iva || 0
            })),
            pagos: pagosFactura
        });

        console.log('Enviando factura:', facturaData);
        console.log('Detalles con IVA:', facturaData.detalles);
        
        // Verificar totales antes de enviar
        const totalesVerificacion = facturacionService.calcularTotalesFactura(facturaData.detalles);
        console.log('Totales calculados antes de enviar:', totalesVerificacion);
        console.log('Total pagos:', facturaData.pagos.reduce((sum, pago) => sum + pago.monto, 0));

        // Crear factura
        const resultado = await facturacionService.crearFacturaCompleta(facturaData);

        Helpers.showToast('Factura creada exitosamente', 'success');
        
        // Limpiar estado temporal primero
        limpiarEstadoTemporal();
        
        // Limpiar formulario
        limpiarFactura();

        // Mostrar información de la factura creada
        mostrarResumenFactura(resultado);

    } catch (error) {
        console.error('Error al guardar factura:', error);
        Helpers.showToast('Error al guardar factura: ' + error.message, 'error');
    } finally {
        mostrarLoading(false);
    }
}

/**
 * Limpiar factura
 */
function limpiarFactura() {
    productosFactura = [];
    pagosFactura = [];
    
    document.getElementById('formFacturacion').reset();
    document.getElementById('buscarProducto').value = '';
    document.getElementById('searchResults').style.display = 'none';
    
    // Limpiar estado temporal
    limpiarEstadoTemporal();
    
    renderizarProductos();
    renderizarMetodosPago();
    actualizarTotales();
    
    Helpers.showToast('Factura limpiada', 'info');
}

/**
 * Mostrar loading
 */
function mostrarLoading(mostrar) {
    const overlay = document.getElementById('loadingOverlay');
    overlay.style.display = mostrar ? 'flex' : 'none';
}

/**
 * Mostrar resumen de factura creada
 */
function mostrarResumenFactura(factura) {
    const mensaje = `
        <div class="text-center">
            <i class="bi bi-check-circle-fill text-success fs-1"></i>
            <h5 class="mt-2">Factura Creada</h5>
            <p><strong>Número:</strong> ${factura.numeroDocumento}</p>
            <p><strong>Total:</strong> ${facturacionService.formatearMoneda(factura.totalNeto)}</p>
            <p><strong>Cliente:</strong> ${factura.nombreTercero}</p>
        </div>
    `;
    
    Helpers.showModal('Factura Exitosa', mensaje);
}

/**
 * Ver facturas de hoy
 */
async function verFacturasHoy() {
    try {
        console.log('=== INICIANDO verFacturasHoy ===');
        
        // Abrir modal específico para facturas de hoy
        const modal = new bootstrap.Modal(document.getElementById('facturasHoyModal'));
        
        // Actualizar mensaje inicial
        const mensajeFacturasHoy = document.getElementById('mensajeFacturasHoy');
        if (mensajeFacturasHoy) {
            mensajeFacturasHoy.innerHTML = `
                <div class="spinner-border spinner-border-sm me-2"></div>
                <strong>Cargando facturas de hoy...</strong>
            `;
            mensajeFacturasHoy.className = 'alert alert-info';
        }
        
        // Mostrar loading en tabla
        const tbody = document.getElementById('cuerpoTablaFacturasHoy');
        tbody.innerHTML = `
            <tr>
                <td colspan="6" class="text-center py-4">
                    <div class="spinner-border text-primary me-2"></div>
                    <strong>Cargando facturas...</strong>
                </td>
            </tr>
        `;
        
        modal.show();
        
        // Usar el endpoint específico de facturas de hoy
        console.log('Llamando a facturacionService.obtenerFacturasHoy()...');
        const facturas = await facturacionService.obtenerFacturasHoy();
        console.log('✅ Facturas recibidas del backend:', facturas);
        console.log('Cantidad de facturas:', facturas ? facturas.length : 0);
        
        if (facturas && facturas.length > 0) {
            // Renderizar tabla específica para facturas de hoy
            renderizarTablaFacturasHoy(facturas);
            
            // Calcular totales del día
            const totalVentas = facturas.reduce((sum, f) => sum + (f.totalNeto || 0), 0);
            const totalBruto = facturas.reduce((sum, f) => sum + (f.totalBruto || 0), 0);
            const totalDescuentos = facturas.reduce((sum, f) => sum + (f.totalDescuentos || 0), 0);
            const totalIVA = facturas.reduce((sum, f) => sum + (f.totalImpu || 0), 0);
            
            console.log('Totales calculados:', {
                cantidad: facturas.length,
                totalVentas,
                totalBruto,
                totalDescuentos,
                totalIVA
            });
            
            // Actualizar mensaje del modal
            if (mensajeFacturasHoy) {
                mensajeFacturasHoy.innerHTML = `
                    <i class="bi bi-calendar-check text-success"></i>
                    <strong>Facturas de Hoy:</strong> ${facturas.length} facturas registradas el ${new Date().toLocaleDateString('es-CO')}
                `;
                mensajeFacturasHoy.className = 'alert alert-success';
            }
            
            // Actualizar resumen
            document.getElementById('resumenCantidad').textContent = facturas.length;
            document.getElementById('resumenTotal').textContent = facturacionService.formatearMoneda(totalVentas);
            document.getElementById('resumenBruto').textContent = facturacionService.formatearMoneda(totalBruto);
            document.getElementById('resumenDescuentos').textContent = facturacionService.formatearMoneda(totalDescuentos);
            document.getElementById('resumenIVA').textContent = facturacionService.formatearMoneda(totalIVA);
            
            Helpers.showToast(
                `📊 Facturas de Hoy: ${facturas.length} | Total: ${facturacionService.formatearMoneda(totalVentas)}`, 
                'success', 
                4000
            );
        } else {
            // No hay facturas hoy
            console.log('ℹ️ No hay facturas registradas hoy');
            
            // Actualizar mensaje
            if (mensajeFacturasHoy) {
                mensajeFacturasHoy.innerHTML = `
                    <i class="bi bi-calendar-x"></i>
                    <strong>Facturas de Hoy:</strong> No hay facturas registradas el ${new Date().toLocaleDateString('es-CO')}
                `;
                mensajeFacturasHoy.className = 'alert alert-info';
            }
            
            // Mostrar mensaje en tabla
            tbody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center text-muted py-4">
                        <i class="bi bi-inbox fs-2"></i>
                        <br>
                        <strong>No hay facturas registradas hoy</strong>
                        <br>
                        <small class="text-muted">${new Date().toLocaleDateString('es-CO', { 
                            weekday: 'long', 
                            year: 'numeric', 
                            month: 'long', 
                            day: 'numeric' 
                        })}</small>
                    </td>
                </tr>
            `;
            
            // Limpiar resumen
            document.getElementById('resumenCantidad').textContent = '0';
            document.getElementById('resumenTotal').textContent = '$0';
            document.getElementById('resumenBruto').textContent = '$0';
            document.getElementById('resumenDescuentos').textContent = '$0';
            document.getElementById('resumenIVA').textContent = '$0';
            
            Helpers.showToast('No hay facturas registradas hoy', 'info');
        }
        
    } catch (error) {
        console.error('❌ Error al obtener facturas de hoy:', error);
        console.error('Stack trace:', error.stack);
        
        // Actualizar mensaje de error
        const mensajeFacturasHoy = document.getElementById('mensajeFacturasHoy');
        if (mensajeFacturasHoy) {
            mensajeFacturasHoy.innerHTML = `
                <i class="bi bi-exclamation-triangle"></i>
                <strong>Error:</strong> No se pudieron cargar las facturas de hoy
                <br>
                <small class="text-muted">${error.message || 'Error desconocido'}</small>
            `;
            mensajeFacturasHoy.className = 'alert alert-danger';
        }
        
        // Mostrar mensaje de error en tabla
        const tbody = document.getElementById('cuerpoTablaFacturasHoy');
        tbody.innerHTML = `
            <tr>
                <td colspan="6" class="text-center text-danger py-4">
                    <i class="bi bi-exclamation-triangle fs-2"></i>
                    <br>
                    <strong>Error al cargar facturas</strong>
                    <br>
                    <small>${error.message || 'Verifique la conexión e intente nuevamente'}</small>
                    <br>
                    <button class="btn btn-outline-primary btn-sm mt-3" onclick="verFacturasHoy()">
                        <i class="bi bi-arrow-clockwise"></i> Reintentar
                    </button>
                </td>
            </tr>
        `;
        
        Helpers.showToast(`Error: ${error.message || 'No se pudieron cargar las facturas'}`, 'error');
    }
}

/**
 * Ver estadísticas
 */
async function verEstadisticas() {
    try {
        const hoy = new Date().toISOString().split('T')[0];
        
        Helpers.showToast('Cargando estadísticas...', 'info');
        
        // Obtener resumen de ventas de hoy
        const resumen = await facturacionService.obtenerResumenVentas(hoy);
        
        if (resumen) {
            const mensaje = `
                📊 Estadísticas de Hoy (${new Date().toLocaleDateString('es-CO')})
                
                💰 Total Ventas: ${facturacionService.formatearMoneda(resumen.totalVentas || 0)}
                📄 Facturas: ${resumen.cantidadFacturas || 0}
                📈 Promedio por Factura: ${facturacionService.formatearMoneda(resumen.promedioFactura || 0)}
            `;
            
            Helpers.showToast(mensaje, 'success', 5000);
        } else {
            Helpers.showToast('No hay estadísticas disponibles para hoy', 'info');
        }
        
    } catch (error) {
        console.error('Error al obtener estadísticas:', error);
        Helpers.showToast('Error al cargar estadísticas', 'error');
    }
}

/**
 * Renderizar tabla específica para facturas de hoy
 */
function renderizarTablaFacturasHoy(facturas) {
    const tbody = document.getElementById('cuerpoTablaFacturasHoy');
    
    if (!facturas || facturas.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center text-muted py-4">
                    <i class="bi bi-inbox fs-2"></i>
                    <br>
                    <strong>No hay facturas registradas hoy</strong>
                </td>
            </tr>
        `;
        return;
    }
    
    tbody.innerHTML = facturas.map(factura => {
        const fecha = new Date(factura.fecha).toLocaleDateString('es-CO');
        const hora = new Date(factura.fecha).toLocaleTimeString('es-CO', { 
            hour: '2-digit', 
            minute: '2-digit' 
        });
        const totalNeto = facturacionService.formatearMoneda(factura.totalNeto || 0);
        const descuentos = facturacionService.formatearMoneda(factura.totalDescuentos || 0);
        const iva = facturacionService.formatearMoneda(factura.totalImpu || 0);
        
        return `
            <tr>
                <td>
                    <strong class="text-primary">${factura.numeroDocumento}</strong>
                    ${factura.observaciones ? `<br><small class="text-muted"><i class="bi bi-chat-text"></i> ${factura.observaciones}</small>` : ''}
                </td>
                <td>
                    <div>${fecha}</div>
                    <small class="text-muted">${hora}</small>
                </td>
                <td>
                    <div class="fw-medium">${factura.nombreTercero}</div>
                </td>
                <td>
                    ${factura.totalDescuentos > 0 ? 
                        `<span class="text-warning fw-bold"><i class="bi bi-percent"></i> ${descuentos}</span>` : 
                        `<span class="text-muted">$0</span>`
                    }
                </td>
                <td>
                    ${factura.totalImpu > 0 ? 
                        `<span class="text-info fw-bold"><i class="bi bi-plus-circle"></i> ${iva}</span>` : 
                        `<span class="text-muted">$0</span>`
                    }
                </td>
                <td>
                    <strong class="text-success">${totalNeto}</strong>
                </td>
                <td>
                    <small class="text-muted"><i class="bi bi-person"></i> ${factura.nombreUsuario}</small>
                </td>
                <td>
                    <div class="btn-group btn-group-sm">
                        <button class="btn btn-outline-primary" onclick="verDetalleFactura(${factura.facturaId})" 
                                title="Ver detalle">
                            <i class="bi bi-eye"></i>
                        </button>
                        <button class="btn btn-outline-success" onclick="imprimirFactura(${factura.facturaId})" 
                                title="Imprimir">
                            <i class="bi bi-printer"></i>
                        </button>
                        <button class="btn btn-outline-danger" onclick="anularFactura(${factura.facturaId})" 
                                title="Anular">
                            <i class="bi bi-x-circle"></i>
                        </button>
                    </div>
                </td>
            </tr>
        `;
    }).join('');
}

/**
 * Buscar facturas
 */
function buscarFacturas() {
    Helpers.showToast('Función de búsqueda en desarrollo', 'info');
}

/**
 * Mostrar ayuda del módulo de facturación
 */
function mostrarAyuda() {
    const ayudaHTML = `
        <div class="text-start">
            <h6><i class="bi bi-info-circle text-primary"></i> Cómo usar el módulo de facturación:</h6>
            <ol>
                <li><strong>Seleccione un cliente</strong> del dropdown</li>
                <li><strong>Busque productos</strong> usando el campo de búsqueda</li>
                <li><strong>Agregue productos</strong> haciendo clic en los resultados</li>
                <li><strong>Ajuste cantidades</strong> y descuentos según necesite</li>
                <li><strong>Agregue métodos de pago</strong> hasta cubrir el total</li>
                <li><strong>Guarde la factura</strong> cuando todo esté correcto</li>
            </ol>
            <hr>
            <h6><i class="bi bi-exclamation-triangle text-warning"></i> Reglas importantes:</h6>
            <ul>
                <li>Monto mínimo: $1,000</li>
                <li>No se permiten productos duplicados</li>
                <li>Los pagos deben coincidir exactamente con el total</li>
                <li>Verifique el stock disponible antes de agregar productos</li>
            </ul>
        </div>
    `;
    
    Helpers.showModal('Ayuda - Facturación', ayudaHTML);
}

// ===== FUNCIONES PARA MODALES DE PRODUCTOS ===== (continuación...)

/**
 * Ver detalle de factura
 */
async function verDetalleFactura(facturaId) {
    try {
        console.log('Viendo detalle de factura:', facturaId);
        Helpers.showToast('Cargando detalle de factura...', 'info');
        
        const factura = await facturacionService.obtenerFacturaCompleta(facturaId);
        
        // Aquí puedes implementar un modal para mostrar el detalle completo
        console.log('Detalle de factura:', factura);
        
        Helpers.showToast('Función de detalle en desarrollo', 'info');
    } catch (error) {
        console.error('Error al ver detalle:', error);
        Helpers.showToast('Error al cargar el detalle de la factura', 'error');
    }
}

/**
 * Imprimir factura
 */
async function imprimirFactura(facturaId) {
    try {
        console.log('Imprimiendo factura:', facturaId);
        Helpers.showToast('Función de impresión en desarrollo', 'info');
    } catch (error) {
        console.error('Error al imprimir:', error);
        Helpers.showToast('Error al imprimir la factura', 'error');
    }
}

/**
 * Anular factura
 */
async function anularFactura(facturaId) {
    try {
        const confirmacion = confirm('¿Está seguro de anular esta factura? Esta acción no se puede deshacer.');
        
        if (confirmacion) {
            const motivo = prompt('Ingrese el motivo de la anulación:');
            
            if (motivo && motivo.trim()) {
                console.log('Anulando factura:', facturaId, 'Motivo:', motivo);
                Helpers.showToast('Función de anulación en desarrollo', 'info');
                
                // await facturacionService.anularFactura(facturaId, motivo);
                // Helpers.showToast('Factura anulada exitosamente', 'success');
                // await verFacturasHoy(); // Recargar la lista
            }
        }
    } catch (error) {
        console.error('Error al anular:', error);
        Helpers.showToast('Error al anular la factura', 'error');
    }
}

/**
 * Exportar facturas de hoy
 */
function exportarFacturasHoy() {
    Helpers.showToast('Función de exportación en desarrollo', 'info');
}

// ===== FUNCIONES PARA MODALES DE PRODUCTOS =====

/**
 * Mostrar todos los productos en un modal
 */
function mostrarTodosLosProductos() {
    const modal = new bootstrap.Modal(document.getElementById('productosModal'));
    const tabla = document.getElementById('tablaProductosModal');
    
    console.log('Mostrando productos en modal. Total:', productos.length);
    if (productos.length > 0) {
        console.log('Primer producto:', productos[0]);
    }
    
    // Actualizar contador de productos
    const contador = document.getElementById('contadorProductosModal');
    if (contador) {
        contador.innerHTML = `<i class="bi bi-box"></i> ${productos.length} productos`;
        contador.className = productos.length > 0 ? 'badge bg-success' : 'badge bg-warning';
    }
    
    if (productos.length === 0) {
        tabla.innerHTML = `
            <tr>
                <td colspan="6" class="text-center text-warning py-5">
                    <i class="bi bi-exclamation-triangle fs-1"></i>
                    <br>
                    <strong class="fs-5">No hay productos cargados</strong>
                    <br>
                    <small class="text-muted">Verifique la conexión con el servidor o ejecute el diagnóstico</small>
                    <br>
                    <div class="mt-3">
                        <button class="btn btn-outline-primary" onclick="inicializarDatos()">
                            <i class="bi bi-arrow-clockwise"></i> Recargar Datos
                        </button>
                    </div>
                </td>
            </tr>
        `;
    } else {
        tabla.innerHTML = productos.map((producto, index) => {
            // Manejar diferentes formatos de propiedades
            const codigo = producto.codigoProducto || producto.codigo_producto || producto.codigo || 'Sin código';
            const nombre = producto.nombre || producto.nombreProducto || producto.descripcion || 'Sin nombre';
            const stock = producto.stockActual || producto.stock_actual || producto.stock || 0;
            const precio = producto.precioUnitario || producto.precio_venta || producto.precioVenta || producto.precio || 0;
            const id = producto.productoId || producto.producto_id || producto.id || index;
            const iva = obtenerPorcentajeIva(producto);
            
            return `
                <tr class="align-middle">
                    <td>
                        <strong class="text-primary">${codigo}</strong>
                    </td>
                    <td>
                        <div class="fw-medium">${nombre}</div>
                        ${producto.categoria ? `<small class="text-muted">${producto.categoria}</small>` : ''}
                    </td>
                    <td class="text-center">
                        <span class="badge ${stock > 10 ? 'bg-success' : stock > 0 ? 'bg-warning text-dark' : 'bg-danger'}">
                            ${stock} unid.
                        </span>
                    </td>
                    <td class="text-end">
                        <strong class="text-success">${facturacionService.formatearMoneda(precio)}</strong>
                    </td>
                    <td class="text-center">
                        <span class="badge ${iva > 0 ? 'bg-info' : 'bg-secondary'}">
                            ${iva > 0 ? `${iva}%` : '0%'}
                        </span>
                    </td>
                    <td class="text-center">
                        ${stock > 0 ? `
                            <button class="btn btn-primary btn-sm" onclick="agregarProductoDesdeModal(${id})" title="Agregar producto">
                                <i class="bi bi-plus-circle"></i> Agregar
                            </button>
                        ` : `
                            <button class="btn btn-secondary btn-sm" disabled title="Sin stock">
                                <i class="bi bi-x-circle"></i> Sin stock
                            </button>
                        `}
                    </td>
                </tr>
            `;
        }).join('');
    }
    
    // Limpiar búsqueda anterior
    document.getElementById('buscarProductoModal').value = '';
    
    // Configurar búsqueda en el modal
    document.getElementById('buscarProductoModal').oninput = (e) => {
        filtrarProductosModal(e.target.value);
    };
    
    modal.show();
}

/**
 * Filtrar productos en el modal
 */
function filtrarProductosModal(termino) {
    const tabla = document.getElementById('tablaProductosModal');
    
    if (!termino) {
        // Mostrar todos los productos usando la misma función
        mostrarTodosLosProductos();
        return;
    }
    
    const terminoLower = termino.toLowerCase();
    const resultados = productos.filter(producto => {
        const codigo = producto.codigo_producto || producto.codigoProducto || producto.codigo || '';
        const nombre = producto.nombre || producto.nombreProducto || producto.descripcion || '';
        
        return codigo.toLowerCase().includes(terminoLower) ||
               nombre.toLowerCase().includes(terminoLower);
    });
    
    console.log(`Filtrado: "${termino}" - ${resultados.length} resultados de ${productos.length} productos`);
    
    // Actualizar contador con resultados filtrados
    const contador = document.getElementById('contadorProductosModal');
    if (contador) {
        contador.innerHTML = `<i class="bi bi-search"></i> ${resultados.length} de ${productos.length}`;
        contador.className = resultados.length > 0 ? 'badge bg-info' : 'badge bg-warning';
    }
    
    if (resultados.length === 0) {
        tabla.innerHTML = `
            <tr>
                <td colspan="6" class="text-center text-muted py-4">
                    <i class="bi bi-search fs-2"></i>
                    <br>
                    <strong>No se encontraron productos</strong>
                    <br>
                    <small>No hay productos que coincidan con "${termino}"</small>
                    <br>
                    <button class="btn btn-outline-secondary btn-sm mt-2" onclick="document.getElementById('buscarProductoModal').value = ''; filtrarProductosModal('');">
                        <i class="bi bi-arrow-left"></i> Mostrar todos
                    </button>
                </td>
            </tr>
        `;
    } else {
        tabla.innerHTML = resultados.map((producto, index) => {
            // Usar la misma lógica de mapeo que en mostrarTodosLosProductos
            const codigo = producto.codigoProducto || producto.codigo_producto || producto.codigo || 'Sin código';
            const nombre = producto.nombre || producto.nombreProducto || producto.descripcion || 'Sin nombre';
            const stock = producto.stockActual || producto.stock_actual || producto.stock || 0;
            const precio = producto.precioUnitario || producto.precio_venta || producto.precioVenta || producto.precio || 0;
            const id = producto.productoId || producto.producto_id || producto.id || index;
            const iva = obtenerPorcentajeIva(producto);
            
            return `
                <tr class="align-middle">
                    <td>
                        <strong class="text-primary">${codigo}</strong>
                    </td>
                    <td>
                        <div class="fw-medium">${nombre}</div>
                        ${producto.categoria ? `<small class="text-muted">${producto.categoria}</small>` : ''}
                    </td>
                    <td class="text-center">
                        <span class="badge ${stock > 10 ? 'bg-success' : stock > 0 ? 'bg-warning text-dark' : 'bg-danger'}">
                            ${stock} unid.
                        </span>
                    </td>
                    <td class="text-end">
                        <strong class="text-success">${facturacionService.formatearMoneda(precio)}</strong>
                    </td>
                    <td class="text-center">
                        <span class="badge ${iva > 0 ? 'bg-info' : 'bg-secondary'}">
                            ${iva > 0 ? `${iva}%` : '0%'}
                        </span>
                    </td>
                    <td class="text-center">
                        ${stock > 0 ? `
                            <button class="btn btn-primary btn-sm" onclick="agregarProductoDesdeModal(${id})" title="Agregar producto">
                                <i class="bi bi-plus-circle"></i> Agregar
                            </button>
                        ` : `
                            <button class="btn btn-secondary btn-sm" disabled title="Sin stock">
                                <i class="bi bi-x-circle"></i> Sin stock
                            </button>
                        `}
                    </td>
                </tr>
            `;
        }).join('');
    }
}

/**
 * Agregar producto desde el modal
 */
function agregarProductoDesdeModal(productoId) {
    const modal = bootstrap.Modal.getInstance(document.getElementById('productosModal'));
    modal.hide();
    agregarProducto(productoId);
}

/**
 * Mostrar modal para agregar producto manual
 */
function agregarProductoManual() {
    const modal = new bootstrap.Modal(document.getElementById('productoManualModal'));
    const select = document.getElementById('productoManualSelect');
    
    // Poblar select con productos
    select.innerHTML = '<option value="">Seleccione un producto...</option>';
    productos.forEach(producto => {
        const id = producto.productoId || producto.producto_id || producto.id;
        const codigo = producto.codigoProducto || producto.codigo_producto || producto.codigo;
        const nombre = producto.nombre || producto.nombreProducto || producto.descripcion;
        const precio = producto.precioUnitario || producto.precio_venta || producto.precio;
        const stock = producto.stockActual || producto.stock_actual || producto.stock;
        const iva = obtenerPorcentajeIva(producto);
        
        const option = document.createElement('option');
        option.value = id;
        option.textContent = `${codigo} - ${nombre}`;
        option.dataset.precio = precio;
        option.dataset.stock = stock;
        option.dataset.iva = iva || 0;
        select.appendChild(option);
    });
    
    // Configurar eventos
    select.onchange = actualizarProductoManual;
    document.getElementById('cantidadManual').oninput = calcularTotalManual;
    document.getElementById('precioManual').oninput = calcularTotalManual;
    document.getElementById('descuentoPorcentajeManual').oninput = calcularTotalManual;
    
    // Limpiar formulario
    document.getElementById('formProductoManual').reset();
    document.getElementById('totalLineaManual').textContent = '$0';
    
    modal.show();
}

/**
 * Actualizar datos cuando se selecciona un producto manual
 */
function actualizarProductoManual() {
    const select = document.getElementById('productoManualSelect');
    const option = select.selectedOptions[0];
    
    if (option && option.value) {
        const precio = parseFloat(option.dataset.precio) || 0;
        const stock = parseInt(option.dataset.stock) || 0;
        const iva = parseFloat(option.dataset.iva) || 0;
        
        document.getElementById('precioManual').value = precio;
        document.getElementById('cantidadManual').max = stock;
        document.getElementById('ivaManual').value = iva;
        
        if (stock === 0) {
            Helpers.showToast('Este producto no tiene stock disponible', 'warning');
        }
        
        calcularTotalManual();
    }
}

/**
 * Calcular total de línea en el modal manual
 */
function calcularTotalManual() {
    const cantidad = parseInt(document.getElementById('cantidadManual').value) || 0;
    const precio = parseFloat(document.getElementById('precioManual').value) || 0;
    const descuentoPorcentaje = parseFloat(document.getElementById('descuentoPorcentajeManual').value) || 0;
    
    const subtotalBruto = cantidad * precio;
    const descuentoTotal = subtotalBruto * (descuentoPorcentaje / 100);
    const totalLinea = subtotalBruto - descuentoTotal;
    
    document.getElementById('totalLineaManual').textContent = facturacionService.formatearMoneda(totalLinea);
}

/**
 * Confirmar producto manual
 */
function confirmarProductoManual() {
    const productoId = document.getElementById('productoManualSelect').value;
    const cantidad = parseInt(document.getElementById('cantidadManual').value);
    const precio = parseFloat(document.getElementById('precioManual').value);
    const descuentoPorcentaje = parseFloat(document.getElementById('descuentoPorcentajeManual').value) || 0;
    
    if (!productoId || !cantidad || !precio) {
        Helpers.showToast('Complete todos los campos requeridos', 'error');
        return;
    }
    
    const producto = productos.find(p => (p.producto_id || p.productoId || p.id) == productoId);
    if (!producto) {
        Helpers.showToast('Producto no encontrado', 'error');
        return;
    }
    
    // Verificar si ya está en la factura
    if (productosFactura.find(p => (p.producto_id || p.productoId || p.id) == productoId)) {
        Helpers.showToast('El producto ya está en la factura', 'warning');
        return;
    }
    
    // Verificar stock
    const stockDisponible = producto.stock_actual || producto.stockActual || producto.stock || 0;
    if (cantidad > stockDisponible) {
        Helpers.showToast(`Stock insuficiente. Disponible: ${stockDisponible}`, 'error');
        return;
    }
    
    // Agregar producto con configuración manual
    const productoFactura = {
        producto_id: producto.producto_id || producto.productoId || producto.id,
        codigo_producto: producto.codigo_producto || producto.codigoProducto || producto.codigo,
        nombre: producto.nombre || producto.nombreProducto || producto.descripcion,
        precio_unitario: precio,
        cantidad: cantidad,
        descuento_porcentaje: descuentoPorcentaje,
        descuento_valor: 0,
        porcentaje_iva: parseFloat(producto.porcentaje_iva || producto.porcentajeIva) || 0,
        stock_disponible: stockDisponible
    };
    
    productosFactura.push(productoFactura);
    guardarEstadoTemporal();
    renderizarProductos();
    actualizarTotales();
    
    const modal = bootstrap.Modal.getInstance(document.getElementById('productoManualModal'));
    modal.hide();
    
    Helpers.showToast(`Producto "${producto.nombre || producto.nombreProducto}" agregado manualmente`, 'success');
}

// ===== FUNCIONES DE RESPALDO TEMPORAL =====

/**
 * Guardar estado temporal de la factura
 */
function guardarEstadoTemporal() {
    try {
        const estado = {
            productosFactura: productosFactura,
            pagosFactura: pagosFactura,
            clienteId: document.getElementById('clienteSelect').value,
            observaciones: document.getElementById('observaciones').value,
            timestamp: new Date().toISOString()
        };
        
        localStorage.setItem('factura_temporal', JSON.stringify(estado));
        console.log('Estado temporal guardado:', estado);
    } catch (error) {
        console.error('Error al guardar estado temporal:', error);
    }
}

/**
 * Recuperar estado temporal de la factura
 */
function recuperarEstadoTemporal() {
    try {
        const estadoStr = localStorage.getItem('factura_temporal');
        if (!estadoStr) return false;
        
        const estado = JSON.parse(estadoStr);
        const ahora = new Date();
        const timestamp = new Date(estado.timestamp);
        const diferenciaHoras = (ahora - timestamp) / (1000 * 60 * 60);
        
        // Solo recuperar si es de las últimas 2 horas
        if (diferenciaHoras > 2) {
            localStorage.removeItem('factura_temporal');
            return false;
        }
        
        // Confirmar recuperación
        const confirmar = confirm(`Se encontró una factura temporal de ${timestamp.toLocaleString()}. ¿Desea recuperarla?`);
        if (!confirmar) {
            localStorage.removeItem('factura_temporal');
            return false;
        }
        
        // Restaurar estado
        productosFactura = estado.productosFactura || [];
        pagosFactura = estado.pagosFactura || [];
        
        if (estado.clienteId) {
            document.getElementById('clienteSelect').value = estado.clienteId;
        }
        
        if (estado.observaciones) {
            document.getElementById('observaciones').value = estado.observaciones;
        }
        
        // Actualizar UI
        renderizarProductos();
        renderizarMetodosPago();
        actualizarTotales();
        
        Helpers.showToast('Factura temporal recuperada', 'success');
        return true;
        
    } catch (error) {
        console.error('Error al recuperar estado temporal:', error);
        localStorage.removeItem('factura_temporal');
        return false;
    }
}

/**
 * Limpiar estado temporal
 */
function limpiarEstadoTemporal() {
    localStorage.removeItem('factura_temporal');
}
