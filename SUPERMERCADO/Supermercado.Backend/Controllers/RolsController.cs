using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolsController : ControllerBase
{
    private readonly DataContext _context;
    public RolsController(DataContext context)
    {
            _context = context; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            return Ok(await _context.Rols.ToListAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> postAsync(Rol rol)
    { 
        try
        {
            _context.Rols.Add(rol);
            await _context.SaveChangesAsync();
            return Ok(rol);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> PutAsync(int id, Rol rol)
    {
        if (id != rol.rol_id)
            return BadRequest("El id del rol no coincide con el id");
        var existingRol = await _context.Rols.FindAsync(id);
        if (existingRol == null)
            return NotFound("El rol no existe");

        existingRol.nombre = rol.nombre;
        try
        {
            await _context.SaveChangesAsync();
            return Ok(existingRol);
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex.Message);
        }
    }

}
