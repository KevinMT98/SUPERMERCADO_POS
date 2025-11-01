using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unit of Work para manejar la lógica de negocio de Movimientos
/// </summary>
public class MovimientoUnitOfWork : GenericUnitOfWork<Movimiento>, IMovimientoUnitOfWork
{
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly IConsecutivoRepository _consecutivoRepository;
    private readonly ITipoDctoRepository _tipoDctoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITerceroRepository _terceroRepository;

    public MovimientoUnitOfWork(
        IMovimientoRepository movimientoRepository,
        IConsecutivoRepository consecutivoRepository,
        ITipoDctoRepository tipoDctoRepository,
        IUsuarioRepository usuarioRepository,
        ITerceroRepository terceroRepository) : base(movimientoRepository)
    {
        _movimientoRepository = movimientoRepository;
        _consecutivoRepository = consecutivoRepository;
        _tipoDctoRepository = tipoDctoRepository;
        _usuarioRepository = usuarioRepository;
        _terceroRepository = terceroRepository;
    }

    /// <summary>
    /// Validaciones de negocio antes de agregar un movimiento
    /// </summary>
    public override async Task<ActionResponse<Movimiento>> AddAsync(Movimiento entity)
    {
        // Validar que el tipo de documento exista
        var tipoDctoResult = await _tipoDctoRepository.GetAsync(entity.FK_codigo_tipodoc);
        if (!tipoDctoResult.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El tipo de documento especificado no existe."
            };
        }

        // Validar que el consecutivo exista
        var consecutivoResult = await _consecutivoRepository.GetAsync(entity.FK_consecutivo_id);
        if (!consecutivoResult.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El consecutivo especificado no existe."
            };
        }

        // Validar que el consecutivo esté dentro del rango
        var consecutivo = consecutivoResult.Result!;
        if (!int.TryParse(entity.numero_documento, out int numeroDoc))
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El número de documento debe ser numérico."
            };
        }

        if (numeroDoc < consecutivo.consecutivo_ini || numeroDoc > consecutivo.consecutivo_fin)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = $"El número de documento debe estar entre {consecutivo.consecutivo_ini} y {consecutivo.consecutivo_fin}."
            };
        }

        // Validar que el usuario exista
        var usuarioResult = await _usuarioRepository.GetAsync(entity.FK_usuario_id);
        if (!usuarioResult.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El usuario especificado no existe."
            };
        }

        // Validar que el tercero exista
        var terceroResult = await _terceroRepository.GetAsync(entity.FK_tercero_id);
        if (!terceroResult.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El tercero especificado no existe."
            };
        }

        // Si todas las validaciones pasan, agregar el movimiento
        return await _movimientoRepository.AddAsync(entity);
    }

    /// <summary>
    /// Validaciones de negocio antes de actualizar un movimiento
    /// </summary>
    public override async Task<ActionResponse<Movimiento>> UpdateAsync(Movimiento entity)
    {
        // Validar que el movimiento exista
        var existingMovimiento = await _movimientoRepository.GetAsync(entity.movimiento_id);
        if (!existingMovimiento.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El movimiento especificado no existe."
            };
        }

        // Aplicar las mismas validaciones que en AddAsync
        var tipoDctoResult = await _tipoDctoRepository.GetAsync(entity.FK_codigo_tipodoc);
        if (!tipoDctoResult.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El tipo de documento especificado no existe."
            };
        }

        var consecutivoResult = await _consecutivoRepository.GetAsync(entity.FK_consecutivo_id);
        if (!consecutivoResult.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El consecutivo especificado no existe."
            };
        }

        var usuarioResult = await _usuarioRepository.GetAsync(entity.FK_usuario_id);
        if (!usuarioResult.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El usuario especificado no existe."
            };
        }

        var terceroResult = await _terceroRepository.GetAsync(entity.FK_tercero_id);
        if (!terceroResult.WasSuccess)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "El tercero especificado no existe."
            };
        }

        return await _movimientoRepository.UpdateAsync(entity);
    }
}
