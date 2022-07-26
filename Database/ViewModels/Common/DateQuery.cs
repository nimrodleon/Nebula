namespace Nebula.Database.ViewModels.Common;

/// <summary>
/// Modelo para el filtro por mes y año.
/// </summary>
public class DateQuery
{
    /// <summary>
    /// Año.
    /// </summary>
    public string Year { get; set; } = string.Empty;

    /// <summary>
    /// Mes.
    /// </summary>
    public string Month { get; set; } = string.Empty;
}
