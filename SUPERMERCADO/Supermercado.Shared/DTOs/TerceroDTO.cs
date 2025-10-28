using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermercado.Shared.DTOs;

public class TerceroDTO
{
    public int tercero_id { get; set; }
    public int codigo_ident { get; set; }
    public string numero_identificacion { get; set; } = null!;
    public string nombre { get; set; } = null!;
    public string? nombre2 { get; set; }
    public string apellido1 { get; set; } = null!;
    public string? apellido2 { get; set; }
    public string email { get; set; } = null!;
    public string direccion { get; set; } = null!;
    public string telefono { get; set; } = null!;
    public bool activo { get; set; } = true;
    public bool es_proveedor { get; set; } = false;
    public bool es_cliente { get; set; } = true;
}

public class TerceroCreateDTO
{
    public int codigo_ident { get; set; }

    [Required(ErrorMessage = "El campo {0} de identificación es obligatorio")]
    public string numero_identificacion { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string nombre { get; set; } = null!;
    public string? nombre2 { get; set; }

    [Required(ErrorMessage = "El campo primer {0} es obligatorio")]
    public string apellido1 { get; set; } = null!;
    public string? apellido2 { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [EmailAddress(ErrorMessage = "El campo {0} no es una dirección de correo válida")]
    public string email { get; set; } = null!;
    public string direccion { get; set; } = null!;
    public string telefono { get; set; } = null!;
    public bool activo { get; set; } = true;
    public bool es_proveedor { get; set; } = false;
    public bool es_cliente { get; set; } = true;
}

public class TerceroUpdateDTO
{
    public int tercero_id { get; set; }
    public int codigo_ident { get; set; }

    [Required(ErrorMessage = "El campo {0} de identificación es obligatorio")]
    public string numero_identificacion { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string nombre { get; set; } = null!;
    public string? nombre2 { get; set; }

    [Required(ErrorMessage = "El campo primer {0} es obligatorio")]
    public string apellido1 { get; set; } = null!;
    public string? apellido2 { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [EmailAddress(ErrorMessage = "El campo {0} no es una dirección de correo válida")]
    public string email { get; set; } = null!;
    public string direccion { get; set; } = null!;
    public string telefono { get; set; } = null!;
    public bool activo { get; set; }
    public bool es_proveedor { get; set; }
    public bool es_cliente { get; set; }
}