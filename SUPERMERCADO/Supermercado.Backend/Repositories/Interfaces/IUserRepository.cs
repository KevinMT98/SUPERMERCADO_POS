using Supermercado.Shared.Responses;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Repositories.Interfaces;

public interface IUserRepository
{
    Task<ActionResponse<Usuario>> GetByEmailAsync(string email);
    Task<ActionResponse<Usuario>> CreateUserAsync(Usuario user);
    Task<bool> ValidatePasswordAsync(Usuario user, string password);

}
