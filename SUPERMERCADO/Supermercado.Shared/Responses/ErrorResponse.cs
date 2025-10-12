namespace Supermercado.Shared.Responses;

/// <summary>
/// Estructura estándar para respuestas de error
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Código de error
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Mensaje de error descriptivo
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Detalles adicionales del error (opcional)
    /// </summary>
    public object? Details { get; set; }

    /// <summary>
    /// Timestamp del error
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Respuesta de error de validación con detalles de campos
/// </summary>
public class ValidationErrorResponse : ErrorResponse
{
    /// <summary>
    /// Errores de validación por campo
    /// </summary>
    public Dictionary<string, string[]> Errors { get; set; } = new();

    public ValidationErrorResponse()
    {
        Code = "VALIDATION_ERROR";
        Message = "Error de validación en los datos enviados";
    }
}
