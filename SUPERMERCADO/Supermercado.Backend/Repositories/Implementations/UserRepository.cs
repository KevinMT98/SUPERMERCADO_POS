using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Supermercado.Backend.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    public UserRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<ActionResponse<Usuario>> GetByEmailAsync(string email)
    {
        try
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
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
                Result = user
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
    public async Task<ActionResponse<Usuario>> CreateUserAsync(Usuario user)
    {
        try
        {
            // Verificar si el email ya está en uso
            var existingUser = await _context.Usuarios.AnyAsync(u => u.email == user.email);
            if (existingUser)
            {
                return new ActionResponse<Usuario>
                {
                    WasSuccess = false,
                    Message = "El email ya está en uso"
                };
            }
            // Hashear la contraseña antes de guardarla
            user.password_hash = BCrypt.Net.BCrypt.HashPassword(user.password_hash);
            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
            return new ActionResponse<Usuario>
            {
                WasSuccess = true,
                Result = user
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
    public async Task<bool> ValidatePasswordAsync(Usuario user, string password)
    {
        // Validar la contraseña usando BCrypt
        return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, user.password_hash));
    }


}
