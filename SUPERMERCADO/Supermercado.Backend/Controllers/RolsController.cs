using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolsController : GenericController<Rol>
{
    public RolsController(IGenericUnitOfWork<Rol> unitOfWork) : base(unitOfWork)
    {

    }
}
