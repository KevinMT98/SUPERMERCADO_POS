using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermercado.Shared.Entities;

public class TipoDcto
{
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Codigo { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Descripcion { get; set; } = null!;

}
