using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unit of Work para manejar la lógica de negocio de Métodos de Pago
/// </summary>
public class MetodosPagoUnitOfWork : GenericUnitOfWork<Metodos_Pago>, IMetodosPagoUnitOfWork
{
    private readonly IMetodosPagoRepository _metodosPagoRepository;

    public MetodosPagoUnitOfWork(IMetodosPagoRepository metodosPagoRepository) : base(metodosPagoRepository)
    {
        _metodosPagoRepository = metodosPagoRepository;
    }

    /// <summary>
    /// Validaciones de negocio antes de agregar un método de pago
    /// </summary>
    public override async Task<ActionResponse<Metodos_Pago>> AddAsync(Metodos_Pago entity)
    {
        // Validar que el código no esté vacío
        if (string.IsNullOrWhiteSpace(entity.codigo_metpag))
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = "El código del método de pago no puede estar vacío."
            };
        }

        // Validar que el nombre no esté vacío
        if (string.IsNullOrWhiteSpace(entity.metodo_pago))
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = "El nombre del método de pago no puede estar vacío."
            };
        }

        // validar que no exista otro método de pago con el mismo código
        var existingMetodoPago = await _metodosPagoRepository.GetByCodigoAsync(entity.codigo_metpag);
        if (existingMetodoPago.WasSuccess)
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = "Ya existe un método de pago con el mismo código."
            };
        }



        return await _metodosPagoRepository.AddAsync(entity);
    }

    /// <summary>
    /// Validaciones de negocio antes de actualizar un método de pago
    /// </summary>
    public override async Task<ActionResponse<Metodos_Pago>> UpdateAsync(Metodos_Pago entity)
    {
        // Validar que el método de pago exista
        var existingMetodoPago = await _metodosPagoRepository.GetAsync(entity.id_metodo_pago);
        if (!existingMetodoPago.WasSuccess)
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = "El método de pago especificado no existe."
            };
        }

        // Validar que el código no esté vacío
        if (string.IsNullOrWhiteSpace(entity.codigo_metpag))
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = "El código del método de pago no puede estar vacío."
            };
        }

        // Validar que el nombre no esté vacío
        if (string.IsNullOrWhiteSpace(entity.metodo_pago))
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = "El nombre del método de pago no puede estar vacío."
            };
        }

        // Validar que no exista otro método de pago con el mismo código (excepto el que se está actualizando)
        var duplicateCheck = await _metodosPagoRepository.GetByCodigoAsync(entity.codigo_metpag);
        if (duplicateCheck.WasSuccess && duplicateCheck.Result!.id_metodo_pago != entity.id_metodo_pago)
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = $"Ya existe otro método de pago con el código '{entity.codigo_metpag}'."
            };
        }

        return await _metodosPagoRepository.UpdateAsync(entity);
    }
}
