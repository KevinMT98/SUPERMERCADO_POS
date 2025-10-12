using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;

/// <summary>
/// DTO de lectura para Tarifa IVA
/// </summary>
public class TarifaIvaDto
{
    public int TarifaIvaId { get; set; }
    public string CodigoIva { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public decimal Porcentaje { get; set; }
    public bool Estado { get; set; }
}

/// <summary>
/// DTO para crear una nueva Tarifa IVA
/// </summary>
public class TarifaIvaCreateDto
{
    [Required(ErrorMessage = "El código IVA es obligatorio")]
    [MaxLength(10, ErrorMessage = "El código IVA no puede tener más de {1} caracteres")]
    public string CodigoIva { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(100, ErrorMessage = "La descripción no puede tener más de {1} caracteres")]
    public string Descripcion { get; set; } = null!;

    [Range(0, 999.99, ErrorMessage = "El porcentaje debe estar entre {1} y {2}")]
    public decimal Porcentaje { get; set; }

    public bool Estado { get; set; } = true;
}

/// <summary>
/// DTO para actualizar una Tarifa IVA existente
/// </summary>
public class TarifaIvaUpdateDto
{
    [Required(ErrorMessage = "El ID de la tarifa IVA es obligatorio")]
    public int TarifaIvaId { get; set; }

    [Required(ErrorMessage = "El código IVA es obligatorio")]
    [MaxLength(10, ErrorMessage = "El código IVA no puede tener más de {1} caracteres")]
    public string CodigoIva { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(100, ErrorMessage = "La descripción no puede tener más de {1} caracteres")]
    public string Descripcion { get; set; } = null!;

    [Range(0, 999.99, ErrorMessage = "El porcentaje debe estar entre {1} y {2}")]
    public decimal Porcentaje { get; set; }

    public bool Estado { get; set; }
}
