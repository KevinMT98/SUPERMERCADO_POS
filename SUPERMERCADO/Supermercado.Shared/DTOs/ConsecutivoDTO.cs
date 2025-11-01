using Supermercado.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermercado.Shared.DTOs;

public class ConsecutivoDTO
{
    public int consecutivo_Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string cod_consecut { get; set; } = null!;
    public int tipodcto { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string descripcion { get; set; } = null!;
    public int consecutivo_ini { get; set; }
    public int consecutivo_fin { get; set; }
    public int consecutivo_actual { get; set; }
    public bool afecta_inv { get; set; }
    public bool es_entrada { get; set; }

}

// DTO para creación
public class ConsecutivoCreateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string cod_consecut { get; set; } = null!;
    public int tipodcto { get; set; }
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string descripcion { get; set; } = null!;
    public int consecutivo_ini { get; set; }
    public int consecutivo_fin { get; set; }
    public int consecutivo_actual { get; set; }
    public bool afecta_inv { get; set; }
    public bool es_entrada { get; set; }
}

// DTO para actualización
public class ConsecutivoUpdateDTO
{
    public int consecutivo_Id { get; set; }
    public string cod_consecut { get; set; } = null!;
    public int tipodcto { get; set; }
    public string descripcion { get; set; } = null!;
    public int consecutivo_ini { get; set; }
    public int consecutivo_fin { get; set; }
    public int consecutivo_actual { get; set; }
    public bool afecta_inv { get; set; }
    public bool es_entrada { get; set; }
}
