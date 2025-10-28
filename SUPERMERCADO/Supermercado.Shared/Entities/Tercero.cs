using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Supermercado.Shared.Entities;

public class Tercero
{
    [Key]
    public int tercero_id { get; set; }
    public int FK_codigo_ident { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string numero_identificacion { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string nombre { get; set; } = null!;
    public string? nombre2 { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string apellido1 { get; set; } = null!;
    public string? apellido2 { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [EmailAddress(ErrorMessage = "El campo {0} debe ser un correo electrónico válido")]
    public string email { get; set; } = null!;
    public string direccion { get; set; } = null!;
    public string telefono { get; set; } = null!;
    public bool activo { get; set; } = true;
    public bool es_proveedor { get; set; } = false;
    public bool es_cliente { get; set; } = true;

    [ForeignKey(nameof(FK_codigo_ident))]
    public  TiposIdentificacion? ID { get; set; }
}

