using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;

/// <summary>
/// DTO para lectura de movimientos
/// </summary>
public class MovimientoDTO
{
    public int movimiento_id { get; set; }
    public int codigo_tipodoc { get; set; }
    public int consecutivo_id { get; set; }
    public string numero_documento { get; set; } = null!;
    public DateTime fecha { get; set; }
    public int usuario_id { get; set; }
    public int tercero_id { get; set; }
    public string? observaciones { get; set; }
}

/// <summary>
/// DTO para creación de movimientos
/// </summary>
public class MovimientoCreateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int codigo_tipodoc { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int consecutivo_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(50, ErrorMessage = "El número de documento no puede exceder {1} caracteres")]
    public string numero_documento { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public DateTime fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int usuario_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int tercero_id { get; set; }

    [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder {1} caracteres")]
    public string? observaciones { get; set; }
}

/// <summary>
/// DTO para actualización de movimientos
/// </summary>
public class MovimientoUpdateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int movimiento_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int codigo_tipodoc { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int consecutivo_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(50, ErrorMessage = "El número de documento no puede exceder {1} caracteres")]
    public string numero_documento { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public DateTime fecha { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int usuario_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int tercero_id { get; set; }

    [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder {1} caracteres")]
    public string? observaciones { get; set; }
}
