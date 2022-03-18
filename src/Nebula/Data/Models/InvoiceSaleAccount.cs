namespace Nebula.Data.Models;

/// <summary>
/// Cuentas por cobrar comprobante de venta.
/// </summary>
public class InvoiceSaleAccount
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// ID Comprobante de Venta.
    /// </summary>
    public string InvoiceSale { get; set; }

    /// <summary>
    /// Serie comprobante.
    /// </summary>
    public string Serie { get; set; }

    /// <summary>
    /// Número comprobante.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Estado Cuenta (PENDIENTE|COBRADO|ANULADO).
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Número de Cuota.
    /// </summary>
    public int Cuota { get; set; }

    /// <summary>
    /// Monto Cuenta.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Saldo de la Cuenta.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Fecha Vencimiento.
    /// </summary>
    public string EndDate { get; set; }

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; }

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; }
}
