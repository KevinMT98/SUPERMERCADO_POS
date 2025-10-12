namespace Supermercado.Shared.DTOs;

/// <summary>
/// DTO para respuestas paginadas
/// </summary>
/// <typeparam name="T">Tipo de los elementos de la lista</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// Lista de elementos de la página actual
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Número de página actual (base 1)
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Tamaño de página (cantidad de elementos por página)
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de elementos en todas las páginas
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Total de páginas
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);

    /// <summary>
    /// Indica si hay página anterior
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Indica si hay página siguiente
    /// </summary>
    public bool HasNextPage => Page < TotalPages;
}

/// <summary>
/// DTO para solicitud de paginación
/// </summary>
public class PaginationRequest
{
    /// <summary>
    /// Número de página (base 1, por defecto 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Tamaño de página (por defecto 10, máximo 100)
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Campo por el cual ordenar
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Dirección de ordenamiento (asc/desc)
    /// </summary>
    public string SortOrder { get; set; } = "asc";

    /// <summary>
    /// Término de búsqueda (opcional)
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Valida y ajusta los valores de paginación
    /// </summary>
    public void Validate()
    {
        if (Page < 1) Page = 1;
        if (PageSize < 1) PageSize = 10;
        if (PageSize > 100) PageSize = 100;
        if (string.IsNullOrWhiteSpace(SortOrder)) SortOrder = "asc";
        SortOrder = SortOrder.ToLower();
    }
}
