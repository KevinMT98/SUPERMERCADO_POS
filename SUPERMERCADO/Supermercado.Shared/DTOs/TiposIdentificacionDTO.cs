using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermercado.Shared.DTOs;

public class TiposIdentificacionDTO
{
    public int ID { get; set; }
    public string? tipoDocumentoID { get; set; }
    public string descripcion { get; set; } = null!;
    public bool activo { get; set; }
}

public class TiposIdentificacionCreateDTO
{
    public string? tipoDocumentoID { get; set; }
    public string descripcion { get; set; } = null!;
    public bool activo { get; set; } = true;
}

public class TiposIdentificacionUpdateDTO
{
    public int ID { get; set; }
    public string? tipoDocumentoID { get; set; }
    public string? descripcion { get; set; } = null!;
    public bool activo { get; set; }
}

public class TiposIdentificacionDeleteDTO
{
    public int ID { get; set; }
}

