using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;

/// <summary>
/// DTO de lectura para Producto
/// </summary>
public class ProductoDto
{
    public int ProductoId { get; set; }
    public string CodigoProducto { get; set; } = null!;
    public string CodigoBarras { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int CategoriaId { get; set; }
    public int CodigoIva { get; set; }
    public int StockActual { get; set; }
    public int StockMinimo { get; set; }
    public int StockMaximo { get; set; }
    public bool Activo { get; set; }
}

/// <summary>
/// DTO para crear un nuevo Producto
/// </summary>
public class ProductoCreateDto
{
    [Required(ErrorMessage = "El código del producto es obligatorio")]
    [MaxLength(50, ErrorMessage = "El código del producto no puede tener más de {1} caracteres")]
    public string CodigoProducto { get; set; } = null!;

    [Required(ErrorMessage = "El código de barras es obligatorio")]
    [MaxLength(100, ErrorMessage = "El código de barras no puede tener más de {1} caracteres")]
    public string CodigoBarras { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(200, ErrorMessage = "El nombre no puede tener más de {1} caracteres")]
    public string Nombre { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "La descripción no puede tener más de {1} caracteres")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio unitario es obligatorio")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio unitario debe estar entre {1} y {2}")]
    public decimal PrecioUnitario { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El código IVA es obligatorio")]
    public int CodigoIva { get; set; }

    [Required(ErrorMessage = "El stock actual es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock actual debe ser mayor o igual a 0")]
    public int StockActual { get; set; }

    [Required(ErrorMessage = "El stock mínimo es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
    public int StockMinimo { get; set; }

    [Required(ErrorMessage = "El stock máximo es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock máximo debe ser mayor o igual a 0")]
    public int StockMaximo { get; set; }

    public bool Activo { get; set; } = true;
}

/// <summary>
/// DTO para actualizar un Producto existente
/// </summary>
public class ProductoUpdateDto
{
    [Required(ErrorMessage = "El ID del producto es obligatorio")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El código del producto es obligatorio")]
    [MaxLength(50, ErrorMessage = "El código del producto no puede tener más de {1} caracteres")]
    public string CodigoProducto { get; set; } = null!;

    [Required(ErrorMessage = "El código de barras es obligatorio")]
    [MaxLength(100, ErrorMessage = "El código de barras no puede tener más de {1} caracteres")]
    public string CodigoBarras { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(200, ErrorMessage = "El nombre no puede tener más de {1} caracteres")]
    public string Nombre { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "La descripción no puede tener más de {1} caracteres")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio unitario es obligatorio")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio unitario debe estar entre {1} y {2}")]
    public decimal PrecioUnitario { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El código IVA es obligatorio")]
    public int CodigoIva { get; set; }

    [Required(ErrorMessage = "El stock actual es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock actual debe ser mayor o igual a 0")]
    public int StockActual { get; set; }

    [Required(ErrorMessage = "El stock mínimo es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
    public int StockMinimo { get; set; }

    [Required(ErrorMessage = "El stock máximo es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock máximo debe ser mayor o igual a 0")]
    public int StockMaximo { get; set; }

    public bool Activo { get; set; }
}