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
            new ItemSituacionFacturador() { id = "01", nombre = "Por Generar XML" },
            new ItemSituacionFacturador() { id = "02", nombre = "XML Generado" },
            new ItemSituacionFacturador() { id = "03", nombre = "Enviado y Aceptado SUNAT" },
            new ItemSituacionFacturador() { id = "04", nombre = "Enviado y Aceptado SUNAT con Obs." },
            new ItemSituacionFacturador() { id = "05", nombre = "Rechazado por SUNAT" },
            new ItemSituacionFacturador() { id = "06", nombre = "Con Errores" },
            new ItemSituacionFacturador() { id = "07", nombre = "Por Validar XML" },
            new ItemSituacionFacturador() { id = "08", nombre = "Enviado a SUNAT Por Procesar" },
            new ItemSituacionFacturador() { id = "09", nombre = "Enviado a SUNAT Procesando" },
            new ItemSituacionFacturador() { id = "10", nombre = "Rechazado por SUNAT" },
            new ItemSituacionFacturador() { id = "11", nombre = "Enviado y Aceptado SUNAT" },
            new ItemSituacionFacturador() { id = "12", nombre = "Enviado y Aceptado SUNAT con Obs." }
        };
    }

    /// <summary>
    /// Obtener una situación en especifico.
    /// </summary>
    /// <param name="id">Identificador de situación</param>
    /// <returns>ItemSituaciónFacturador|null</returns>
    public ItemSituacionFacturador? GetItemSituaciónFacturador(string id)
    {
        return ListaDeSituacionesFacturador.FirstOrDefault(x => x.id == id);
    }
}
