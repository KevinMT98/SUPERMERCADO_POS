using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio para operaciones CRUD de usuarios
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly DataContext _context;

    public UsuarioRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ActionResponse<IEnumerable<Usuario>>> GetAsync()
    {
        try
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .OrderBy(u => u.nombre)
                .ToListAsync();

            return new ActionResponse<IEnumerable<Usuario>>
            {
                WasSuccess = true,
                Result = usuarios
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<IEnumerable<Usuario>>
            {
                WasSuccess = false,
                Message = $"Error al obtener los usuarios: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<Usuario>> GetAsync(int id)
    {
        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.usuario_id == id);

            if (usuario == null)
            {
                return new ActionResponse<Usuario>
                {
                    WasSuccess = false,
                    Message = "Usuario no encontrado"
                };
            }

            return new ActionResponse<Usuario>
            {
                WasSuccess = true,
                Result = usuario
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Usuario>
            {
                WasSuccess = false,
                Message = $"Error al obtener el usuario: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<Usuario>> AddAsync(Usuario usuario)
    {
        try
        {
            // Hash de la contraseña antes de guardar
            usuario.password_hash = BCrypt.Net.BCrypt.HashPassword(usuario.password_hash);
            
            // Establecer fecha de creación con zona horaria de Bogotá, Colombia (UTC-5)
            var colombiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            usuario.fecha_creacion = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, colombiaTimeZone);
            usuario.activo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Cargar el rol para la respuesta
            await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();

            return new ActionResponse<Usuario>
            {
                WasSuccess = true,
                Result = usuario
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Usuario>
            {
                WasSuccess = false,
                Message = "Ya existe un registro con los mismos datos."
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Usuario>
            {
                WasSuccess = false,
                Message = $"Error al crear el usuario: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<Usuario>> UpdateAsync(Usuario usuario)
    {
        try
        {
            var existingUser = await _context.Usuarios.FindAsync(usuario.usuario_id);
            if (existingUser == null)
            {
                return new ActionResponse<Usuario>
                {
                    WasSuccess = false,
                    Message = "Usuario no encontrado"
                };
            }

            // Actualizar campos básicos
            existingUser.nombre_usuario = usuario.nombre_usuario;
            existingUser.nombre = usuario.nombre;
            existingUser.apellido = usuario.apellido;
            existingUser.email = usuario.email;
            existingUser.FK_rol_id = usuario.FK_rol_id;
            existingUser.activo = usuario.activo;
            
            // Solo actualizar contraseña si no está vacía
            if (!string.IsNullOrWhiteSpace(usuario.password_hash))
            {
                existingUser.password_hash = BCrypt.Net.BCrypt.HashPassword(usuario.password_hash);
            }
            
            // Establecer fecha de modificación con zona horaria de Bogotá, Colombia (UTC-5)
            var colombiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            existingUser.fecha_modificacion = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, colombiaTimeZone);

            _context.Usuarios.Update(existingUser);
            await _context.SaveChangesAsync();

            // Cargar el rol para la respuesta
            await _context.Entry(existingUser).Reference(u => u.Rol).LoadAsync();

            return new ActionResponse<Usuario>
            {
                WasSuccess = true,
                Result = existingUser
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Usuario>
            {
                WasSuccess = false,
                Message = "Ya existe un registro con los mismos datos."
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Usuario>
            {
                WasSuccess = false,
                Message = $"Error al actualizar el usuario: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<Usuario>> DeleteAsync(int id)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return new ActionResponse<Usuario>
                {
                    WasSuccess = false,
                    Message = "Usuario no encontrado"
                };
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return new ActionResponse<Usuario>
            {
                WasSuccess = true
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Usuario>
            {
                WasSuccess = false,
                Message = "No se puede eliminar el usuario porque tiene registros relacionados."
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Usuario>
            {
                WasSuccess = false,
                Message = $"Error al eliminar el usuario: {ex.Message}"
            };
        }
    }

    public async Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.nombre_usuario == nombreUsuario && u.usuario_id != excludeId.Value);
        }

        return await _context.Usuarios
            .AnyAsync(u => u.nombre_usuario == nombreUsuario);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.email == email && u.usuario_id != excludeId.Value);
        }

        return await _context.Usuarios
            .AnyAsync(u => u.email == email);
    }
}
