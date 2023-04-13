namespace Nebula.Database.Dto.Common;

/// <summary>
/// Modelo para el filtro por mes y año.
/// </summary>
public class DateQuery
{
    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = string.Empty;

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = string.Empty;

    /// <summary>
    /// Texto de consulta.
    /// </summary>
    public string? Query { get; set; } = string.Empty;
}
