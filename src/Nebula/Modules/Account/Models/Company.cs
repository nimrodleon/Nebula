using Nebula.Modules.Auth.Models;
using Nebula.Modules.Contacts.Models;
using Nebula.Modules.InvoiceHub.Dto;
using Nebula.Modules.InvoiceHub.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Account.Models;

public class Company
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador del dueño de la empresa.
    /// </summary>
    public Guid? UserId { get; set; } = null;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = new User();

    /// <summary>
    /// R.U.C. Empresa.
    /// </summary>
    public string Ruc { get; set; } = string.Empty;

    /// <summary>
    /// Razón Social.
    /// </summary>
    public string RznSocial { get; set; } = string.Empty;

    /// <summary>
    /// Dirección Empresa.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono Empresa.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Ancho de la impresión del Ticket.
    /// </summary>
    public string AnchoTicket { get; set; } = string.Empty;

    /// <summary>
    /// Código Local Emisor.
    /// </summary>
    public string CodLocalEmisor { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de moneda.
    /// </summary>
    public string TipMoneda { get; set; } = string.Empty;

    /// <summary>
    /// Porcentaje IGV.
    /// </summary>
    public decimal PorcentajeIgv { get; set; }

    /// <summary>
    /// Monto Impuesto a la Bolsa plástica.
    /// </summary>
    public decimal ValorImpuestoBolsa { get; set; }

    /// <summary>
    /// Contacto por defecto para operaciones
    /// menores a 700 soles con boleta.
    /// </summary>
    public Guid? ContactId { get; set; } = null;

    [ForeignKey(nameof(ContactId))]
    public Contact Contact { get; set; } = new Contact();

    /// <summary>
    /// #Dias para créditos automáticos.
    /// </summary>
    public int DiasPlazo { get; set; } = 0;

    /// <summary>
    /// Codigo ubigeo del domicilio fiscal.
    /// </summary>
    public string Ubigueo { get; set; } = string.Empty;

    /// <summary>
    /// En que departamento del perú se encuentra.
    /// </summary>
    public string Departamento { get; set; } = string.Empty;

    /// <summary>
    /// En que provincia se encuentra.
    /// </summary>
    public string Provincia { get; set; } = string.Empty;

    /// <summary>
    /// En que distrito se encuentra.
    /// </summary>
    public string Distrito { get; set; } = string.Empty;

    /// <summary>
    /// En que urbanización está la dirección.
    /// </summary>
    public string Urbanizacion { get; set; } = "";

    /// <summary>
    /// Fecha vencimiento del certificado.
    /// </summary>
    public string FechaVencimientoCert { get; set; } = "-";

    /// <summary>
    /// Tipo de endpoint SUNAT, FE_BETA | FE_PRODUCCION
    /// </summary>
    public string SunatEndpoint { get; set; } = SunatEndpoints.FeBeta;

    /// <summary>
    /// Configuración de la Clave SOL.
    /// </summary>
    public ClaveSolHub ClaveSol { get; set; } = new ClaveSolHub();

    /// <summary>
    /// Módulo de inventarios.
    /// </summary>
    public bool ModInventarios { get; set; } = false;

    /// <summary>
    /// Módulo comprobantes.
    /// </summary>
    public bool ModComprobantes { get; set; } = false;

    /// <summary>
    /// Módulo cuentas por cobrar.
    /// </summary>
    public bool ModCuentaPorCobrar { get; set; } = false;

    /// <summary>
    /// Módulo reparaciones.
    /// </summary>
    public bool ModReparaciones { get; set; } = false;

    /// <summary>
    /// Módulo cajas diaria.
    /// </summary>
    public bool ModCajasDiaria { get; set; } = false;

    /// <summary>
    /// Configura la emisión de comprobantes para,
    /// Las empresas del regimen único simplificado.
    /// </summary>
    public bool EmitirModoRus { get; set; } = false;

    /// <summary>
    /// Registrar aqui el último pago realizado.
    /// </summary>
    public string PagoSuscripcionId { get; set; } = string.Empty;
}
