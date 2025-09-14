using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Categoria_ProductoController : GenericController<Categoria_Producto>
{
    public Categoria_ProductoController(IGenericUnitOfWork<Categoria_Producto> unitOfWork) : base(unitOfWork)
    {

    }
}
