using MongoDB.Bson.Serialization.Attributes;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Contacts.Models;
using Nebula.Modules.Sales.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Finanzas.Models;

/// <summary>
/// Cuentas por Cobrar.
/// </summary>
public class FinancialAccount
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
    /// Tipo Operación: 'CARGO' | 'ABONO'
    /// </summary>
    public string Type { get; set; } = "CARGO";

    /// <summary>
    /// Identificador del Contacto.
    /// </summary>
    public Guid? ContactId { get; set; } = null;

    [ForeignKey(nameof(ContactId))]
    public Contact Contact { get; set; } = new Contact();

    /// <summary>
    /// Nombre de Contacto.
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Observación / Comentario.
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// Clave foránea comprobante de venta.
    /// </summary>
    public Guid? InvoiceSaleId { get; set; } = null;

    [ForeignKey(nameof(InvoiceSaleId))]
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();

    /// <summary>
    /// Configura el Tipo de comprobante.
    /// </summary>
    public string DocType { get; set; } = "-";

    /// <summary>
    /// Documento de Venta: F001-1.
    /// </summary>
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// Forma de Pago: Yape | Depósito | Contado.
    /// </summary>
    public string FormaPago { get; set; } = string.Empty;

    /// <summary>
    /// Monto de la deuda.
    /// </summary>
    public decimal Cargo { get; set; }

    /// <summary>
    /// Monto del Pago de la deuda.
    /// </summary>
    public decimal Abono { get; set; }

    /// <summary>
    /// Saldo pendiente a Cobrar.
    /// </summary>
    [BsonIgnore]
    public decimal Saldo { get; set; }

    /// <summary>
    /// Estado del Cargo: 'PENDIENTE' | 'COBRADO' | '-'.
    /// </summary>
    public string Status { get; set; } = "PENDIENTE";

    /// <summary>
    /// Identificador del Terminal de Venta.
    /// </summary>
    public Guid? CajaDiariaId { get; set; } = null;

    [ForeignKey(nameof(CajaDiariaId))]
    public CajaDiaria CajaDiaria { get; set; } = new CajaDiaria();

    /// <summary>
    /// Nombre del Terminal de Venta.
    /// </summary>
    public string Terminal { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Cargo.
    /// </summary>
    public Guid? FinancialAccountId { get; set; } = null;

    /// <summary>
    /// Fecha de Operación.
    /// </summary>
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

    /// <summary>
    /// Fecha de Vencimiento a Pagar.
    /// </summary>
    public string EndDate { get; set; } = "-";

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
