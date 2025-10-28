using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermercado.Shared.Entities;

public class TiposIdentificacion
{
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage ="El campo {0} es obligatorio")]
    public string? tipoDocumentoID { get; set; }

    [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1}")]
    public string descripcion { get; set; } = null!;
    public bool activo { get; set; } = true;

}
