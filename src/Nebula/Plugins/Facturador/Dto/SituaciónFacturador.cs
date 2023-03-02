namespace Nebula.Plugins.Facturador.Dto;

/// <summary>
/// Situación del Facturador.
/// </summary>
public class SituaciónFacturador
{
    /// <summary>
    /// Valores devueltos por el facturador,
    /// después de realizar cada operación.
    /// </summary>
    public List<ItemSituacionFacturador> ListaDeSituacionesFacturador { get; set; }

    /// <summary>
    /// constructor de clase.
    /// </summary>
    public SituaciónFacturador()
    {
        ListaDeSituacionesFacturador = new List<ItemSituacionFacturador>()
        {
            new ItemSituacionFacturador() { Id = "01", Nombre = "Por Generar XML" },
            new ItemSituacionFacturador() { Id = "02", Nombre = "XML Generado" },
            new ItemSituacionFacturador() { Id = "03", Nombre = "Enviado y Aceptado SUNAT" },
            new ItemSituacionFacturador() { Id = "04", Nombre = "Enviado y Aceptado SUNAT con Obs." },
            new ItemSituacionFacturador() { Id = "05", Nombre = "Rechazado por SUNAT" },
            new ItemSituacionFacturador() { Id = "06", Nombre = "Con Errores" },
            new ItemSituacionFacturador() { Id = "07", Nombre = "Por Validar XML" },
            new ItemSituacionFacturador() { Id = "08", Nombre = "Enviado a SUNAT Por Procesar" },
            new ItemSituacionFacturador() { Id = "09", Nombre = "Enviado a SUNAT Procesando" },
            new ItemSituacionFacturador() { Id = "10", Nombre = "Rechazado por SUNAT" },
            new ItemSituacionFacturador() { Id = "11", Nombre = "Enviado y Aceptado SUNAT" },
            new ItemSituacionFacturador() { Id = "12", Nombre = "Enviado y Aceptado SUNAT con Obs." }
        };
    }

    /// <summary>
    /// Obtener una situación en especifico.
    /// </summary>
    /// <param name="id">Identificador de situación</param>
    /// <returns>ItemSituaciónFacturador|null</returns>
    public ItemSituacionFacturador? GetItemSituaciónFacturador(string id)
    {
        return ListaDeSituacionesFacturador.FirstOrDefault(x => x.Id == id);
    }
}
