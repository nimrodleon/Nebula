namespace Nebula.Data.ViewModels;

/// <summary>
/// Modelo para el filtro por mes y año.
/// </summary>
public class DateQuery
{
    /// <summary>
    /// Año.
    /// </summary>
    public string Year { get; set; }

    /// <summary>
    /// Mes.
    /// </summary>
    public string Month { get; set; }
}
