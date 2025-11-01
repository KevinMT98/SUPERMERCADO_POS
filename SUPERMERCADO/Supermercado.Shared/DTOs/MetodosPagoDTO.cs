using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;

/// <summary>
/// DTO para lectura de métodos de pago
/// </summary>
public class MetodosPagoDTO
{
    public int id_metodo_pago { get; set; }
    public string metodo_pago { get; set; } = null!;
    public string codigo_metpag { get; set; } = null!;
    public bool activo { get; set; }
}

/// <summary>
/// DTO para creación de métodos de pago
/// </summary>
public class MetodosPagoCreateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(100, ErrorMessage = "El {0} no puede exceder {1} caracteres")]
    public string metodo_pago { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(20, ErrorMessage = "El {0} no puede exceder {1} caracteres")]
    public string codigo_metpag { get; set; } = null!;

    public bool activo { get; set; } = true;
}

/// <summary>
/// DTO para actualización de métodos de pago
/// </summary>
public class MetodosPagoUpdateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int id_metodo_pago { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(100, ErrorMessage = "El {0} no puede exceder {1} caracteres")]
    public string metodo_pago { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(20, ErrorMessage = "El {0} no puede exceder {1} caracteres")]
    public string codigo_metpag { get; set; } = null!;

    public bool activo { get; set; }
}
