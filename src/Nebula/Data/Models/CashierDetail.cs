namespace Nebula.Data.Models;

public enum TypeOperation
{
    CajaChica,
    Comprobante
}

/// <summary>
/// Detalle de caja diaria.
/// </summary>
public class CashierDetail
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador CajaDiaria.
    /// </summary>
    public string CajaDiaria { get; set; }

    /// <summary>
    /// Serie y Número de documento.
    /// </summary>
    public string Document { get; set; } = "-";

    /// <summary>
    /// Nombre Contacto.
    /// </summary>
    public string Contact { get; set; } = "-";

    /// <summary>
    /// Observación de la Operación.
    /// </summary>
    public string Remark { get; set; } = "-";

    /// <summary>
    /// Movimiento de efectivo,
    /// (ENTRADA|SALIDA).
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Tipo de Operación.
    /// </summary>
    public TypeOperation TypeOperation { get; set; }

    /// <summary>
    /// Forma de Pago (Credito|Contado).
    /// </summary>
    public string FormaPago { get; set; }

    /// <summary>
    /// Monto de la Operación.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Hora de la Operación.
    /// </summary>
    public string Hour { get; set; } = DateTime.Now.ToString("HH:mm");

    /// <summary>
    /// Fecha de Operación.
    /// </summary>
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
