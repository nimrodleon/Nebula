using Nebula.Modules.Account.Models;
using Nebula.Modules.Contacts.Models;
using Nebula.Modules.Sales.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Cashier.Models;

/// <summary>
/// Detalle de caja diaria.
/// </summary>
public class CashierDetail
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    public Guid? CompanyId { get; set; } = null;

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();

    /// <summary>
    /// Identificador CajaDiaria.
    /// </summary>
    public Guid? CajaDiariaId { get; set; } = null;

    [ForeignKey(nameof(CajaDiariaId))]
    public CajaDiaria CajaDiaria { get; set; } = new CajaDiaria();

    /// <summary>
    /// Clave foránea comprobante de venta.
    /// </summary>
    public Guid? InvoiceSaleId { get; set; } = null;

    [ForeignKey(nameof(InvoiceSaleId))]
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();

    /// <summary>
    /// Configura el Tipo de comprobante.
    /// </summary>
    public string DocType { get; set; } = "NOTA";

    /// <summary>
    /// Serie y Número de documento.
    /// </summary>
    public string Document { get; set; } = "-";

    /// <summary>
    /// Identificador de Contacto.
    /// </summary>
    public Guid? ContactId { get; set; } = null;

    [ForeignKey(nameof(ContactId))]
    public Contact Contact { get; set; } = new Contact();

    /// <summary>
    /// Nombre de Contacto.
    /// </summary>
    public string ContactName { get; set; } = "-";

    /// <summary>
    /// Observación de la Operación.
    /// </summary>
    public string Remark { get; set; } = "-";

    /// <summary>
    /// Definir Tipo de registro.
    /// </summary>
    public string TypeOperation { get; set; } = string.Empty;

    /// <summary>
    /// Forma de Pago (Credito|Contado).
    /// </summary>
    public string FormaPago { get; set; } = string.Empty;

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
