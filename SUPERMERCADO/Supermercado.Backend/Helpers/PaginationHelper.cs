using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermercado.Shared.DTOs;

namespace Supermercado.Backend.Helpers;

/// <summary>
/// Helper para manejar paginación con encabezado X-Total-Count
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Aplica paginación a una consulta IQueryable y agrega el encabezado X-Total-Count
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="query">Consulta IQueryable</param>
    /// <param name="request">Parámetros de paginación</param>
    /// <param name="response">HttpResponse para agregar el encabezado</param>
    /// <returns>Respuesta paginada</returns>
    public static async Task<PaginatedResponse<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        PaginationRequest request,
        HttpResponse response)
    {
        // Validar parámetros de paginación
        request.Validate();

        // Obtener el total de elementos
        var total = await query.CountAsync();

        // Agregar encabezado X-Total-Count
        response.Headers.Append("X-Total-Count", total.ToString());

        // Calcular skip
        var skip = (request.Page - 1) * request.PageSize;

        // Aplicar paginación
        var items = await query
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync();

        return new PaginatedResponse<T>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            Total = total
        };
    }

    /// <summary>
    /// Crea una respuesta paginada desde una lista ya cargada en memoria
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="items">Lista de elementos</param>
    /// <param name="request">Parámetros de paginación</param>
    /// <param name="response">HttpResponse para agregar el encabezado</param>
    /// <returns>Respuesta paginada</returns>
    public static PaginatedResponse<T> Paginate<T>(
        this IEnumerable<T> items,
        PaginationRequest request,
        HttpResponse response)
    {
        // Validar parámetros de paginación
        request.Validate();

        var list = items.ToList();
        var total = list.Count;

        // Agregar encabezado X-Total-Count
        response.Headers.Append("X-Total-Count", total.ToString());

        // Calcular skip
        var skip = (request.Page - 1) * request.PageSize;

        // Aplicar paginación
        var pagedItems = list
            .Skip(skip)
            .Take(request.PageSize)
            .ToList();

        return new PaginatedResponse<T>
        {
            Items = pagedItems,
            Page = request.Page,
            PageSize = request.PageSize,
            Total = total
        };
    }

    /// <summary>
    /// Agrega el encabezado X-Total-Count a la respuesta
    /// </summary>
    /// <param name="response">HttpResponse</param>
    /// <param name="total">Total de elementos</param>
    public static void AddTotalCountHeader(this HttpResponse response, int total)
    {
        response.Headers.Append("X-Total-Count", total.ToString());
    }
}
