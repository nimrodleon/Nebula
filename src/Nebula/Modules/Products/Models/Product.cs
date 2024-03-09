using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Products.Models;

public class Product
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
    /// Descripción del producto.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Categoría de producto.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Código de Barra.
    /// </summary>
    public string Barcode { get; set; } = "-";

    /// <summary>
    /// Tipo IGV Sunat.
    /// </summary>
    public string IgvSunat { get; set; } = TipoIGV.Gravado;

    /// <summary>
    /// Precio Venta Unitario (Incluye IGV).
    /// </summary>
    public decimal PrecioVentaUnitario { get; set; }

    /// <summary>
    /// Tipo de Bien/Servicio.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Unidad de Medida.
    /// </summary>
    public string UndMedida { get; set; } = string.Empty;
}
