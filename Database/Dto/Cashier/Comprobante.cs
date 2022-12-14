namespace Nebula.Database.Dto.Cashier;

public class Comprobante
{
    /// <summary>
    /// id contacto.
    /// </summary>
    public string ContactId { get; set; } = string.Empty;

    /// <summary>
    /// ID del Comprobante.
    /// </summary>
    public string InvoiceSale { get; set; } = string.Empty;

    /// <summary>
    /// forma de pago. (Yape|Contado|Depósito).
    /// </summary>
    public string FormaPago { get; set; } = string.Empty;

    /// <summary>
    /// tipo documento. ('BOLETA'|'FACTURA'|'NOTA').
    /// </summary>
    public string DocType { get; set; } = string.Empty;

    /// <summary>
    /// monto recibido al momento de cobrar.
    /// </summary>
    public decimal MontoRecibido { get; set; } = 0;

    /// <summary>
    /// vuelto de la venta.
    /// </summary>
    public decimal Vuelto { get; set; } = 0;

    /// <summary>
    /// observación o comentario.
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// subTotal de la venta.
    /// </summary>
    public decimal SumTotValVenta { get; set; } = 0;

    /// <summary>
    /// monto del tributo IGV(18%).
    /// </summary>
    public decimal SumTotTributos { get; set; } = 0;

    /// <summary>
    /// importe total cobrado.
    /// </summary>
    public decimal SumImpVenta { get; set; } = 0;

    /// <summary>
    /// impuesto a la bolsa plástica.
    /// </summary>
    public decimal SumTotTriIcbper { get; set; } = 0;
}
