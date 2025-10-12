using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;

public class UserDTO
{
    public int UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool Activo { get; set; }
    public DateOnly FechaCreacion { get; set; }
    public DateOnly FechaModificacion { get; set; }
    public int Rol { get; set; }
}

public class UserCreateDTO
{
    [Required(ErrorMessage = "El Nombre del usuario es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre del usuario no puede tener más de {1} caracteres")]
    public string NombreUsuario { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos {1} caracteres")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "El Nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede tener más de {1} caracteres")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El Apellido es obligatorio")]
    [MaxLength(100, ErrorMessage = "El apellido no puede tener más de {1} caracteres")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El Email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El Rol es obligatorio")]
    public int Rol { get; set; }

    public bool Activo { get; set; } = true;
}

public class UserUpdateDTO
{
    [Required(ErrorMessage = "El Id del usuario es obligatorio")]
    public int UsuarioId { get; set; }
    [Required(ErrorMessage = "El Nombre del usuario es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre del usuario no puede tener más de {1} caracteres")]
    public string NombreUsuario { get; set; } = null!;
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos {1} caracteres")]
    public string? Password { get; set; }
    [Required(ErrorMessage = "El Nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede tener más de {1} caracteres")]
    public string Nombre { get; set; } = null!;
    [Required(ErrorMessage = "El Apellido es obligatorio")]
    [MaxLength(100, ErrorMessage = "El apellido no puede tener más de {1} caracteres")]
    public string Apellido { get; set; } = null!;
    [Required(ErrorMessage = "El Email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "El Rol es obligatorio")]
    public int Rol { get; set; }
    [Required(ErrorMessage = "El estado Activo es obligatorio")]
    public bool Activo { get; set; }
}