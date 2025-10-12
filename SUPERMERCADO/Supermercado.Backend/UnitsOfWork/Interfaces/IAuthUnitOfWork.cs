using Supermercado.Shared.DTOs;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;


public interface IAuthUnitOfWork
{
    Task<ActionResponse<TokenDTO>> LoginAsync(LoginDTO dto);
}
