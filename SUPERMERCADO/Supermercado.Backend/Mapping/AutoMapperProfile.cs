using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // ===== MAPEOS PARA TARIFA IVA =====
        
        // Mapeo de Entidad a DTO de lectura
        CreateMap<Tarifa_IVA, TarifaIvaDto>()
            .ForMember(dest => dest.TarifaIvaId, opt => opt.MapFrom(src => src.tarifa_iva_id))
            .ForMember(dest => dest.CodigoIva, opt => opt.MapFrom(src => src.codigo_Iva))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(dest => dest.Porcentaje, opt => opt.MapFrom(src => src.porcentaje))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.estado));


        // Mapeo de DTO de creación a Entidad
        CreateMap<TarifaIvaCreateDto, Tarifa_IVA>()
            .ForMember(dest => dest.codigo_Iva, opt => opt.MapFrom(src => src.CodigoIva))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.porcentaje, opt => opt.MapFrom(src => src.Porcentaje))
            .ForMember(dest => dest.estado, opt => opt.MapFrom(src => src.Estado))
            .ForMember(dest => dest.tarifa_iva_id, opt => opt.Ignore()); // El ID se genera automáticamente

        // Mapeo de DTO de actualización a Entidad
        CreateMap<TarifaIvaUpdateDto, Tarifa_IVA>()
            .ForMember(dest => dest.tarifa_iva_id, opt => opt.MapFrom(src => src.TarifaIvaId))
            .ForMember(dest => dest.codigo_Iva, opt => opt.MapFrom(src => src.CodigoIva))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.porcentaje, opt => opt.MapFrom(src => src.Porcentaje))
            .ForMember(dest => dest.estado, opt => opt.MapFrom(src => src.Estado));

        // ===== MAPEOS PARA PRODUCTO =====
        
        // Mapeo de Entidad a DTO de lectura
        CreateMap<Producto, ProductoDto>()
            .ForMember(dest => dest.ProductoId, opt => opt.MapFrom(src => src.producto_id))
            .ForMember(dest => dest.CodigoProducto, opt => opt.MapFrom(src => src.codigo_producto))
            .ForMember(dest => dest.CodigoBarras, opt => opt.MapFrom(src => src.codigo_barras))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(dest => dest.PrecioUnitario, opt => opt.MapFrom(src => src.precio_unitario))
            .ForMember(dest => dest.CategoriaId, opt => opt.MapFrom(src => src.FK_categoria_id))
            .ForMember(dest => dest.CodigoIva, opt => opt.MapFrom(src => src.FK_codigo_iva))
            .ForMember(dest => dest.StockActual, opt => opt.MapFrom(src => src.stock_actual))
            .ForMember(dest => dest.StockMinimo, opt => opt.MapFrom(src => src.stock_minimo))
            .ForMember(dest => dest.StockMaximo, opt => opt.MapFrom(src => src.stock_maximo))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.activo));

        // Mapeo de DTO de creación a Entidad
        CreateMap<ProductoCreateDto, Producto>()
            .ForMember(dest => dest.codigo_producto, opt => opt.MapFrom(src => src.CodigoProducto))
            .ForMember(dest => dest.codigo_barras, opt => opt.MapFrom(src => src.CodigoBarras))
            .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.precio_unitario, opt => opt.MapFrom(src => src.PrecioUnitario))
            .ForMember(dest => dest.FK_categoria_id, opt => opt.MapFrom(src => src.CategoriaId))
            .ForMember(dest => dest.FK_codigo_iva, opt => opt.MapFrom(src => src.CodigoIva))
            .ForMember(dest => dest.stock_actual, opt => opt.MapFrom(src => src.StockActual))
            .ForMember(dest => dest.stock_minimo, opt => opt.MapFrom(src => src.StockMinimo))
            .ForMember(dest => dest.stock_maximo, opt => opt.MapFrom(src => src.StockMaximo))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.Activo))
            .ForMember(dest => dest.producto_id, opt => opt.Ignore()) // El ID se genera automáticamente
            .ForMember(dest => dest.Categoria, opt => opt.Ignore()) // Propiedades de navegación
            .ForMember(dest => dest.TarifaIVA, opt => opt.Ignore());

        // Mapeo de DTO de actualización a Entidad
        CreateMap<ProductoUpdateDto, Producto>()
            .ForMember(dest => dest.producto_id, opt => opt.MapFrom(src => src.ProductoId))
            .ForMember(dest => dest.codigo_producto, opt => opt.MapFrom(src => src.CodigoProducto))
            .ForMember(dest => dest.codigo_barras, opt => opt.MapFrom(src => src.CodigoBarras))
            .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.precio_unitario, opt => opt.MapFrom(src => src.PrecioUnitario))
            .ForMember(dest => dest.FK_categoria_id, opt => opt.MapFrom(src => src.CategoriaId))
            .ForMember(dest => dest.FK_codigo_iva, opt => opt.MapFrom(src => src.CodigoIva))
            .ForMember(dest => dest.stock_actual, opt => opt.MapFrom(src => src.StockActual))
            .ForMember(dest => dest.stock_minimo, opt => opt.MapFrom(src => src.StockMinimo))
            .ForMember(dest => dest.stock_maximo, opt => opt.MapFrom(src => src.StockMaximo))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.Activo))
            .ForMember(dest => dest.Categoria, opt => opt.Ignore()) // Propiedades de navegación
            .ForMember(dest => dest.TarifaIVA, opt => opt.Ignore());

        //  ===== MAPEOS PARA USUARIOS =====

        // Mapeo de Entidad a DTO de lectura

        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.usuario_id))
            .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.nombre_usuario))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.FK_rol_id))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.activo));
        //.ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol != null ? src.Rol.nombre : string.Empty));

        // Mapeo de DTO de creación a Entidad
        CreateMap<UsuarioCreateDto, Usuario>()
            .ForMember(dest => dest.nombre_usuario, opt => opt.MapFrom(src => src.NombreUsuario))
            .ForMember(dest => dest.password_hash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.apellido, opt => opt.MapFrom(src => src.Apellido))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FK_rol_id, opt => opt.MapFrom(src => src.Rol))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.Activo))
            .ForMember(dest => dest.fecha_creacion, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.fecha_modificacion, opt => opt.Ignore())
            .ForMember(dest => dest.usuario_id, opt => opt.Ignore())
            .ForMember(dest => dest.Rol, opt => opt.Ignore());

        // Mapeo de DTO de actualización a Entidad

        CreateMap<UsuarioUpdateDto, Usuario>()
            .ForMember(dest => dest.usuario_id, opt => opt.MapFrom(src => src.UsuarioId))
            .ForMember(dest => dest.nombre_usuario, opt => opt.MapFrom(src => src.NombreUsuario))
            .ForMember(dest => dest.password_hash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.apellido, opt => opt.MapFrom(src => src.Apellido))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FK_rol_id, opt => opt.MapFrom(src => src.Rol))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.Activo))
            .ForMember(dest => dest.fecha_creacion, opt => opt.Ignore())
            .ForMember(dest => dest.fecha_modificacion, opt => opt.Ignore())
            .ForMember(dest => dest.Rol, opt => opt.Ignore());



        // ===== MAPEOS PARA TIPODOS DE IDENTIFICACION =====

        CreateMap<TiposIdentificacion, TiposIdentificacionDTO>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.tipoDocumentoID, opt => opt.MapFrom(src => src.tipoDocumentoID))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.activo));

        // Mapeo de DTO de creación a Entidad
        CreateMap<TiposIdentificacionCreateDTO, TiposIdentificacion>()
            .ForMember(dest => dest.tipoDocumentoID, opt => opt.MapFrom(src => src.tipoDocumentoID))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.activo));

        // Mapeo de DTO de actualización a Entidad
        CreateMap<TiposIdentificacionUpdateDTO, TiposIdentificacion>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.tipoDocumentoID, opt => opt.MapFrom(src => src.tipoDocumentoID))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.activo));


        //  ===== MAPEOS PARA TIPODOS DE IDENTIFICACION =====

        CreateMap<Tercero, TerceroDTO>()
            .ForMember(dest => dest.tercero_id, opt => opt.MapFrom(src => src.tercero_id))
            .ForMember(dest => dest.codigo_ident, opt => opt.MapFrom(src => src.FK_codigo_ident))
            .ForMember(dest => dest.numero_identificacion, opt => opt.MapFrom(src => src.numero_identificacion))
            .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.nombre))
            .ForMember(dest => dest.nombre2, opt => opt.MapFrom(src => src.nombre2))
            .ForMember(dest => dest.apellido1, opt => opt.MapFrom(src => src.apellido1))
            .ForMember(dest => dest.apellido2, opt => opt.MapFrom(src => src.apellido2))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.direccion, opt => opt.MapFrom(src => src.direccion))
            .ForMember(dest => dest.telefono, opt => opt.MapFrom(src => src.telefono))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.activo))
            .ForMember(dest => dest.es_cliente, opt => opt.MapFrom(src => src.es_cliente))
            .ForMember(dest => dest.es_proveedor, opt => opt.MapFrom(src => src.es_proveedor));

        // Mapeo de DTO de creación a Entidad

        CreateMap<TerceroCreateDTO, Tercero>()
            .ForMember(dest => dest.FK_codigo_ident, opt => opt.MapFrom(src => src.codigo_ident))
            .ForMember(dest => dest.numero_identificacion, opt => opt.MapFrom(src => src.numero_identificacion))
            .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.nombre))
            .ForMember(dest => dest.nombre2, opt => opt.MapFrom(src => src.nombre2))
            .ForMember(dest => dest.apellido1, opt => opt.MapFrom(src => src.apellido1))
            .ForMember(dest => dest.apellido2, opt => opt.MapFrom(src => src.apellido2))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.direccion, opt => opt.MapFrom(src => src.direccion))
            .ForMember(dest => dest.telefono, opt => opt.MapFrom(src => src.telefono))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.activo))
            .ForMember(dest => dest.es_cliente, opt => opt.MapFrom(src => src.es_cliente))
            .ForMember(dest => dest.es_proveedor, opt => opt.MapFrom(src => src.es_proveedor));

        // Mapeo de DTO de actualización a Entidad
        CreateMap<TerceroUpdateDTO, Tercero>()
            .ForMember(dest => dest.tercero_id, opt => opt.MapFrom(src => src.tercero_id))
            .ForMember(dest => dest.FK_codigo_ident, opt => opt.MapFrom(src => src.codigo_ident))
            .ForMember(dest => dest.numero_identificacion, opt => opt.MapFrom(src => src.numero_identificacion))
            .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.nombre))
            .ForMember(dest => dest.nombre2, opt => opt.MapFrom(src => src.nombre2))
            .ForMember(dest => dest.apellido1, opt => opt.MapFrom(src => src.apellido1))
            .ForMember(dest => dest.apellido2, opt => opt.MapFrom(src => src.apellido2))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.direccion, opt => opt.MapFrom(src => src.direccion))
            .ForMember(dest => dest.telefono, opt => opt.MapFrom(src => src.telefono))
            .ForMember(dest => dest.activo, opt => opt.MapFrom(src => src.activo))
            .ForMember(dest => dest.es_cliente, opt => opt.MapFrom(src => src.es_cliente))
            .ForMember(dest => dest.es_proveedor, opt => opt.MapFrom(src => src.es_proveedor));



        // ===== MAPEOS PARA CONSECUTIVO =====
        // Entidad a DTO de lectura
        CreateMap<Consecutivo, ConsecutivoDTO>()
            .ForMember(dest => dest.consecutivo_Id, opt => opt.MapFrom(src => src.consecutivo_Id))
            .ForMember(dest => dest.cod_consecut, opt => opt.MapFrom(src => src.cod_consecut))
            .ForMember(dest => dest.tipodcto, opt => opt.MapFrom(src => src.FK_codigo_tipodcto))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(dest => dest.consecutivo_ini, opt => opt.MapFrom(src => src.consecutivo_ini))
            .ForMember(dest => dest.consecutivo_fin, opt => opt.MapFrom(src => src.consecutivo_fin))
            .ForMember(dest => dest.consecutivo_actual, opt => opt.MapFrom(src => src.consecutivo_actual))
            .ForMember(dest => dest.afecta_inv, opt => opt.MapFrom(src => src.afecta_inv))
            .ForMember(dest => dest.es_entrada, opt => opt.MapFrom(src => src.es_entrada));

        // DTO de creación a entidad
        CreateMap<ConsecutivoCreateDTO, Consecutivo>()
            .ForMember(dest => dest.cod_consecut, opt => opt.MapFrom(src => src.cod_consecut))
            .ForMember(dest => dest.FK_codigo_tipodcto, opt => opt.MapFrom(src => src.tipodcto))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(dest => dest.consecutivo_ini, opt => opt.MapFrom(src => src.consecutivo_ini))
            .ForMember(dest => dest.consecutivo_fin, opt => opt.MapFrom(src => src.consecutivo_fin))
            .ForMember(dest => dest.consecutivo_actual, opt => opt.MapFrom(src => src.consecutivo_actual))
            .ForMember(dest => dest.afecta_inv, opt => opt.MapFrom(src => src.afecta_inv))
            .ForMember(dest => dest.es_entrada, opt => opt.MapFrom(src => src.es_entrada))
            .ForMember(dest => dest.consecutivo_Id, opt => opt.Ignore()); // El ID se genera automáticamente

        // DTO de actualización a entidad
        CreateMap<ConsecutivoUpdateDTO, Consecutivo>()
            .ForMember(dest => dest.consecutivo_Id, opt => opt.MapFrom(src => src.consecutivo_Id))
            .ForMember(dest => dest.cod_consecut, opt => opt.MapFrom(src => src.cod_consecut))
            .ForMember(dest => dest.FK_codigo_tipodcto, opt => opt.MapFrom(src => src.tipodcto))
            .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(dest => dest.consecutivo_ini, opt => opt.MapFrom(src => src.consecutivo_ini))
            .ForMember(dest => dest.consecutivo_fin, opt => opt.MapFrom(src => src.consecutivo_fin))
            .ForMember(dest => dest.consecutivo_actual, opt => opt.MapFrom(src => src.consecutivo_actual))
            .ForMember(dest => dest.afecta_inv, opt => opt.MapFrom(src => src.afecta_inv))
            .ForMember(dest => dest.es_entrada, opt => opt.MapFrom(src => src.es_entrada));



    }


}
