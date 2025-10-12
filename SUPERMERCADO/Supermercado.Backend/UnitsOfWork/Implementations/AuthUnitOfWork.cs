using Microsoft.IdentityModel.Tokens;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unidad de trabajo para gestionar operaciones de autenticación y autorización
/// </summary>
public class AuthUnitOfWork : IAuthUnitOfWork
{
    private readonly IUserRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthUnitOfWork(IUserRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// Autentica un usuario y genera un token JWT
    /// </summary>
    /// <param name="dto">Datos de login (email y contraseña)</param>
    /// <returns>Token JWT con información del usuario autenticado</returns>
    public async Task<ActionResponse<TokenDTO>> LoginAsync(LoginDTO dto)
    {
        try
        {
            // Validar que el usuario existe por email
            var userResponse = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if (!userResponse.WasSuccess || userResponse.Result == null)
            {
                return new ActionResponse<TokenDTO>
                {
                    WasSuccess = false,
                    Message = "Credenciales inválidas"
                };
            }

            var user = userResponse.Result;

            // Verificar que el usuario esté activo
            if (!user.activo)
            {
                return new ActionResponse<TokenDTO>
                {
                    WasSuccess = false,
                    Message = "El usuario no está activo. Contacte al administrador del sistema."
                };
            }

            // Validar la contraseña
            var isValidPassword = await _usuarioRepository.ValidatePasswordAsync(user, dto.Password);
            if (!isValidPassword)
            {
                return new ActionResponse<TokenDTO>
                {
                    WasSuccess = false,
                    Message = "Credenciales inválidas"
                };
            }

            // Obtener el nombre del rol para el token
            var roleName = user.Rol?.nombre ?? "User";

            // Generar el token JWT
            var token = GenerateJwtToken(user.email ?? string.Empty, roleName, user.usuario_id.ToString());
            var expiredIn = 3600; // 1 hora en segundos

            return new ActionResponse<TokenDTO>
            {
                WasSuccess = true,
                Result = new TokenDTO
                {
                    AccesToken = token,
                    ExpireIn = expiredIn,
                    Email = user.email ?? string.Empty,
                    Role = roleName
                }
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<TokenDTO>
            {
                WasSuccess = false,
                Message = $"Error al autenticar el usuario: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Genera un token JWT con los claims del usuario
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <param name="roleName">Nombre del rol del usuario</param>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Token JWT como string</returns>
    private string GenerateJwtToken(string email, string roleName, string userId)
    {
        // Obtener configuración JWT desde appsettings.json con valores por defecto seguros
        var jwtKey = _configuration["Jwt:Key"] ?? "SuperSecretKey_ChangeInProduction_MinLength32Characters!";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "SupermercadoAPI";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "SupermercadoAPIClient";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Crear claims estándar para el usuario autenticado
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, roleName),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, 
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // Usar UTC para evitar problemas de zona horaria
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


