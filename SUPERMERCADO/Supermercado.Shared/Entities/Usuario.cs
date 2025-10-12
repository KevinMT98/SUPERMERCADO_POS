using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermercado.Shared.Entities;

/// <summary>
/// Entidad que representa un usuario del sistema
/// </summary>

public class Usuario
{
    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    [Key]
    public int usuario_id { get; set; }

    /// <summary>
    /// Nombre de usuario único para login
    /// </summary>
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")]
    [Column("nombre_usuario")]
    public string nombre_usuario { get; set; } = null!;

    /// <summary>
    /// Hash de la contraseña del usuario
    /// </summary>
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(255, ErrorMessage = "La contraseña no puede exceder 255 caracteres")]
    [Column("password_hash")]
    public string password_hash { get; set; } = null!;

    /// <summary>
    /// Nombre del usuario
    /// </summary>
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    [Column("nombre")]
    public string nombre { get; set; } = null!;

    /// <summary>
    /// Apellido del usuario
    /// </summary>
    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
    [Column("apellido")]
    public string apellido { get; set; } = null!;

    /// <summary>
    /// Email del usuario
    /// </summary>
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    [Column("email")]
    public string? email { get; set; }

    /// <summary>
    /// Indica si el usuario está activo
    /// </summary>
    [Column("activo")]
    public bool activo { get; set; } = true;

    /// <summary>
    /// Fecha de creación del usuario
    /// </summary>
    [Column("fecha_creacion")]
    public DateTime fecha_creacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de última modificación
    /// </summary>
    [Column("fecha_modificacion")]
    public DateTime? fecha_modificacion { get; set; }

    /// <summary>
    /// Clave foránea del rol
    /// </summary>
    [Required(ErrorMessage = "El rol es obligatorio")]
    [Column("FK_rol_id")]
    public int FK_rol_id { get; set; }

    /// <summary>
    /// Navegación al rol del usuario
    /// </summary>
    [ForeignKey("FK_rol_id")]
    public Rol? Rol { get; set; }
}
