using Supermercado.Shared.Entities;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

/// <summary>
/// Interfaz para el Unit of Work de Facturas
/// </summary>
public interface IFacturaUnitOfWork : IGenericUnitOfWork<Factura>
{
    // Métodos específicos de lógica de negocio si son necesarios
}
