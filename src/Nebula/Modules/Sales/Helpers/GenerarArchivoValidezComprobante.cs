using System.Globalization;
using System.IO.Compression;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Helpers;

public class GenerarArchivoValidezComprobante
{
    private readonly List<InvoiceSerie> _invoiceSeries;
    private readonly List<InvoiceSale> _invoiceSales;
    private readonly List<CreditNote> _creditNotes;

    /// <summary>
    /// Cargar Lista de Comprobantes a Exportar.
    /// </summary>
    /// <param name="invoiceSeries">Series de Facturación</param>
    /// <param name="invoiceSales">Boletas/Facturas</param>
    /// <param name="creditNotes">Notas de Crédito</param>
    public GenerarArchivoValidezComprobante(List<InvoiceSerie> invoiceSeries,
        List<InvoiceSale> invoiceSales, List<CreditNote> creditNotes)
    {
        _invoiceSeries = invoiceSeries;
        _invoiceSales = invoiceSales;
        _creditNotes = creditNotes;
    }

    /// <summary>
    /// Obtener Lista de Comprobantes por Tipo y serie.
    /// </summary>
    /// <param name="type">BOLETA|FACTURA</param>
    /// <param name="serie">Serie Comprobante</param>
    /// <returns>Lista de Comprobantes</returns>
    private List<InvoiceSale> GetComprobantesByTypeAndSerie(string type, string serie)
    {
        return _invoiceSales.Where(x => x.DocType.Equals(type) && x.Serie.Equals(serie)).ToList();
    }

    /// <summary>
    /// Obtener Lista de Notas de crédito según serie.
    /// </summary>
    /// <param name="serie">Serie Comprobante</param>
    /// <returns>Lista de Notas de Crédito</returns>
    private List<CreditNote> GetNotaDeCrédito(string serie)
    {
        return _creditNotes.Where(x => x.Serie.Equals(serie)).ToList();
    }

    /// <summary>
    /// Crear un directorio en la carpeta temporal del usuario.
    /// </summary>
    /// <returns>Ruta del Directorio Creado!</returns>
    private string CrearCarpetaTemporal()
    {
        string tempPath = Path.GetTempPath();
        string dirName = Guid.NewGuid().ToString();
        string fullPath = Path.Combine(tempPath, dirName);
        // crear directorio temporal.
        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
        // crear directorio de comprobantes.
        if (Directory.Exists(fullPath))
        {
            string pathComprobantes = Path.Combine(fullPath, "Comprobantes");
            if (!Directory.Exists(pathComprobantes))
                Directory.CreateDirectory(pathComprobantes);
        }

        return fullPath;
    }

    /// <summary>
    /// Comprimir la Carpeta de Comprobantes Generados.
    /// </summary>
    /// <param name="pathCarpetaZip">Path de la carpeta temporal.</param>
    /// <returns>path del archivo comprimido</returns>
    private string ComprimirCarpetaComprobantes(string pathCarpetaZip)
    {
        string pathArchivoComprimido = Path.Combine(pathCarpetaZip, "Comprobantes.zip");
        string pathCarpetaComprobante = Path.Combine(pathCarpetaZip, "Comprobantes");
        // Obtener la lista de archivos planos a incluir en el zip.
        string[] listaDeArchivosPlanos = Directory.GetFiles(pathCarpetaComprobante, "*.txt");
        // Crear el archivo zip y agregar los archivos planos.
        using ZipArchive archive = ZipFile.Open(pathArchivoComprimido, ZipArchiveMode.Create);
        foreach (string archivo in listaDeArchivosPlanos)
            archive.CreateEntryFromFile(archivo, Path.GetFileName(archivo));
        return pathArchivoComprimido;
    }

    /// <summary>
    /// Generar archivo plano comprobantes.
    /// </summary>
    /// <param name="pathComprobante">path de la carpeta temporal</param>
    /// <param name="rucEmisor">ruc del emisor</param>
    /// <param name="type">tipo comprobante BOLETA|FACTURA</param>
    /// <param name="serie">serie del comprobante</param>
    private void GenerarArchivoPlanoComprobantes(string pathComprobante, string rucEmisor, string type, string serie)
    {
        string docType = string.Empty;
        if (type.Equals("FACTURA")) docType = "01";
        if (type.Equals("BOLETA")) docType = "03";
        List<InvoiceSale> comprobantes = GetComprobantesByTypeAndSerie(type, serie);
        // Dividir la lista en grupos de 100 comprobantes.
        var gruposDeComprobantes = comprobantes
            .Select((c, i) => new { Comprobante = c, Indice = i })
            .GroupBy(x => x.Indice / 100)
            .Select(g => g.Select(x => x.Comprobante));
        // Guardar cada grupo en un archivo separado.
        int numeroDeGrupo = 1;
        foreach (var grupo in gruposDeComprobantes)
        {
            var numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
            numberFormatInfo.NumberGroupSeparator = string.Empty;
            string nombreDeArchivo = $"{type}_{serie}_{numeroDeGrupo}.txt";
            string pathArchivoPlano = Path.Combine(pathComprobante, nombreDeArchivo);
            using StreamWriter streamWriter = new StreamWriter(pathArchivoPlano);
            foreach (var comprobante in grupo)
            {
                DateTime date = DateTime.Parse(comprobante.FecEmision);
                string fecEmision = date.ToString("dd/MM/yyyy");
                string sumImpVenta = comprobante.SumImpVenta.ToString("N2", numberFormatInfo);
                if (comprobante == grupo.Last())
                    streamWriter.Write(
                        $"{rucEmisor}|{docType}|{comprobante.Serie}|{comprobante.Number}|{fecEmision}|{sumImpVenta}");
                else
                    streamWriter.WriteLine(
                        $"{rucEmisor}|{docType}|{comprobante.Serie}|{comprobante.Number}|{fecEmision}|{sumImpVenta}");
            }

            numeroDeGrupo++;
        }
    }

    /// <summary>
    /// Generar Archivo Plano notas de Crédito.
    /// </summary>
    /// <param name="pathComprobante">path de la carpeta temporal</param>
    /// <param name="rucEmisor">ruc del emisor</param>
    /// <param name="serie">serie del comprobante</param>
    private void GenerarArchivoPlanoNotaDeCrédito(string pathComprobante, string rucEmisor, string serie)
    {
        List<CreditNote> notasDeCrédito = GetNotaDeCrédito(serie);
        // Dividir la lista en grupos de 100 comprobantes.
        var gruposDeComprobantes = notasDeCrédito
            .Select((c, i) => new { Comprobante = c, Indice = i })
            .GroupBy(x => x.Indice / 100)
            .Select(g => g.Select(x => x.Comprobante));
        // Guardar cada grupo en un archivo separado.
        int numeroDeGrupo = 1;
        foreach (var grupo in gruposDeComprobantes)
        {
            var numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
            numberFormatInfo.NumberGroupSeparator = string.Empty;
            string nombreDeArchivo = $"notas_crédito_{serie}_{numeroDeGrupo}.txt";
            string pathArchivoPlano = Path.Combine(pathComprobante, nombreDeArchivo);
            using StreamWriter streamWriter = new StreamWriter(pathArchivoPlano);
            foreach (var comprobante in grupo)
            {
                DateTime date = DateTime.Parse(comprobante.FecEmision);
                string fecEmision = date.ToString("dd/MM/yyyy");
                string sumImpVenta = comprobante.SumImpVenta.ToString("N2", numberFormatInfo);
                if (comprobante == grupo.Last())
                    streamWriter.Write(
                        $"{rucEmisor}|07|{comprobante.Serie}|{comprobante.Number}|{fecEmision}|{sumImpVenta}");
                else
                    streamWriter.WriteLine(
                        $"{rucEmisor}|07|{comprobante.Serie}|{comprobante.Number}|{fecEmision}|{sumImpVenta}");
            }

            numeroDeGrupo++;
        }
    }

    /// <summary>
    /// Crea Todos los archivos de validación.
    /// </summary>
    /// <param name="rucEmisor">Ruc del Emisor</param>
    /// <returns>path del archivo comprimido</returns>
    public string CrearArchivosDeValidación(string rucEmisor)
    {
        string pathResult = string.Empty;
        string tempPath = CrearCarpetaTemporal();
        if (Directory.Exists(tempPath))
        {
            string carpetaDeTrabajo = Path.Combine(tempPath, "Comprobantes");
            _invoiceSeries.ForEach(item =>
            {
                // generar archivos planos de comprobantes.
                GenerarArchivoPlanoComprobantes(carpetaDeTrabajo, rucEmisor, "BOLETA", item.Boleta);
                GenerarArchivoPlanoComprobantes(carpetaDeTrabajo, rucEmisor, "FACTURA", item.Factura);
                GenerarArchivoPlanoNotaDeCrédito(carpetaDeTrabajo, rucEmisor, item.CreditNoteBoleta);
                GenerarArchivoPlanoNotaDeCrédito(carpetaDeTrabajo, rucEmisor, item.CreditNoteFactura);
            });
            // comprimir carpeta de comprobantes.
            pathResult = ComprimirCarpetaComprobantes(tempPath);
        }

        return pathResult;
    }
}
