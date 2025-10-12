using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermercado.Shared.DTOs;

public class LoginDTO
{

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [EmailAddress(ErrorMessage = "El campo {0} debe ser un email válido")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Password { get; set; } = null!;
}

public class  TokenDTO
{
    public string AccesToken { get; set; } = null!;
    public int ExpireIn { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;


}