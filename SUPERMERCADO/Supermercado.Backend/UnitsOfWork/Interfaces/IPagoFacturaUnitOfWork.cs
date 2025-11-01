using Supermercado.Shared.Entities;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

/// <summary>
/// Interfaz para el Unit of Work de Pago de Factura
/// </summary>
public interface IPagoFacturaUnitOfWork : IGenericUnitOfWork<Pago_Factura>
{
    // Métodos específicos de lógica de negocio si son necesarios
}
