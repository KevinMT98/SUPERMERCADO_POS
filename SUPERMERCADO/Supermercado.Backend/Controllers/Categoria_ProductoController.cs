using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.UinitsOfWork.Interfaces;
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
