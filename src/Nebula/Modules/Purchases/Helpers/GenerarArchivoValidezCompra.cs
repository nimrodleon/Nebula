using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Purchases.Models;
using Nebula.Modules.Sales.Models;
using System.Globalization;
using System.IO.Compression;

namespace Nebula.Modules.Purchases.Helpers;

public class GenerarArchivoValidezCompra
{
    private readonly List<PurchaseInvoice> _purchases;

    public GenerarArchivoValidezCompra(List<PurchaseInvoice> purchases)
    {
        _purchases = purchases;
    }

    private List<PurchaseInvoice> GetComprobantesByType(string type)
    {
        return _purchases.Where(x => x.DocType == type).ToList();
    }

    private string CrearCarpetaTemporal()
    {
        string tempPath = Path.GetTempPath();
        string dirName = Guid.NewGuid().ToString();
        string fullPath = Path.Combine(tempPath, dirName);
        // crear directorio temporal.
        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);
        // crear directorio de compras.
        if (Directory.Exists(fullPath))
        {
            string pathComprobantes = Path.Combine(fullPath, "Compras");
            if (!Directory.Exists(pathComprobantes))
                Directory.CreateDirectory(pathComprobantes);
        }

        return fullPath;
    }

    private string ComprimirCarpetaCompras(string pathCarpetaZip)
    {
        string pathArchivoComprimido = Path.Combine(pathCarpetaZip, "Compras.zip");
        string pathCarpetaCompras = Path.Combine(pathCarpetaZip, "Compras");
        // Obtener la lista de archivos planos a incluir en el zip.
        string[] listaDeArchivosPlanos = Directory.GetFiles(pathCarpetaCompras, "*.txt");
        // Crear el archivo zip y agregar los archivos planos.
        using ZipArchive archive = ZipFile.Open(pathArchivoComprimido, ZipArchiveMode.Create);
        foreach (string archivo in listaDeArchivosPlanos)
            archive.CreateEntryFromFile(archivo, Path.GetFileName(archivo));
        return pathArchivoComprimido;
    }

    private void GenerarArchivoPlanoCompras(string pathComprobante, string type)
    {
        string docType = string.Empty;
        if (type.Equals("FACTURA")) docType = "01";
        if (type.Equals("BOLETA")) docType = "03";
        List<PurchaseInvoice> compras = GetComprobantesByType(type);
        // Dividir la lista en grupos de 100 comprobantes.
        var gruposDeComprobantes = compras
            .Select((c, i) => new { Comprobante = c, Indice = i })
            .GroupBy(x => x.Indice / 100)
            .Select(g => g.Select(x => x.Comprobante));
        // Guardar cada grupo en un archivo separado.
        int numeroDeGrupo = 1;
        foreach (var grupo in gruposDeComprobantes)
        {
            var numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
            numberFormatInfo.NumberGroupSeparator = string.Empty;
            string nombreDeArchivo = $"{type}_{numeroDeGrupo}.txt";
            string pathArchivoPlano = Path.Combine(pathComprobante, nombreDeArchivo);
            using StreamWriter streamWriter = new StreamWriter(pathArchivoPlano);
            foreach (var comprobante in grupo)
            {
                DateTime date = DateTime.Parse(comprobante.FecEmision);
                string fecEmision = date.ToString("dd/MM/yyyy");
                string sumImpCompra = comprobante.SumImpCompra.ToString("N2", numberFormatInfo);
                if (comprobante == grupo.Last())
                    streamWriter.Write(
                        $"{comprobante.NumDocProveedor}|{docType}|{comprobante.Serie}|{comprobante.Number}|{fecEmision}|{sumImpCompra}");
                else
                    streamWriter.WriteLine(
                        $"{comprobante.NumDocProveedor}|{docType}|{comprobante.Serie}|{comprobante.Number}|{fecEmision}|{sumImpCompra}");
            }

            numeroDeGrupo++;
        }
    }

    public string CrearArchivosDeValidacion()
    {
        string pathResult = string.Empty;
        string tempPath = CrearCarpetaTemporal();
        if (Directory.Exists(tempPath))
        {
            string carpetaDeTrabajo = Path.Combine(tempPath, "Compras");
            GenerarArchivoPlanoCompras(carpetaDeTrabajo, "BOLETA");
            GenerarArchivoPlanoCompras(carpetaDeTrabajo, "FACTURA");
            // comprimir carpeta de comprobantes.
            pathResult = ComprimirCarpetaCompras(tempPath);
        }

        return pathResult;
    }

}
