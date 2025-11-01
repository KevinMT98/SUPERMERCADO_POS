using Supermercado.Shared.Entities;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

/// <summary>
/// Interfaz para el Unit of Work de Movimientos
/// </summary>
public interface IMovimientoUnitOfWork : IGenericUnitOfWork<Movimiento>
{
    // Aquí se pueden agregar métodos específicos de lógica de negocio si son necesarios
}
