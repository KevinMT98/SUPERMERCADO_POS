using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Repositories.Interfaces;

/// <summary>
/// Interfaz para el repositorio de Movimientos
/// </summary>
public interface IMovimientoRepository : IGenericRepository<Movimiento>
{
    // Aquí se pueden agregar métodos específicos para Movimiento si son necesarios
    // Por ejemplo: obtener movimientos por fecha, por tercero, etc.
}
