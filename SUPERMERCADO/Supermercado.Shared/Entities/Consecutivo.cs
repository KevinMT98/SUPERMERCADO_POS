using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermercado.Shared.Entities;

public class Consecutivo
{
    [Key]
    public int consecutivo_Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string? cod_consecut {  get; set; }
    public int FK_codigo_tipodcto { get; set; }

    [Required(ErrorMessage ="El campo {0} es obligatorio")]
    public string? descripcion { get; set; }
    public int consecutivo_ini {  get; set; }
    public int consecutivo_fin { get; set; }
    public int consecutivo_actual { get; set; }
    public bool afecta_inv {  get; set; }
    public bool es_entrada { get; set; }

    // Propiedades de navegación
    [ForeignKey(nameof(FK_codigo_tipodcto))]
    public TipoDcto? ID { get; set; }

}
