namespace Nebula.Database.Services.Facturador;

public static class FacturadorControl
{
    public static void CrearDirectorioControl(string storagePath, string year, string month)
    {
        string carpetaFacturador = Path.Combine(storagePath, "facturador");
        string carpetaData = Path.Combine(carpetaFacturador, "DATA", year, month);
        string carpetaEnvio = Path.Combine(carpetaFacturador, "ENVIO", year, month);
        string carpetaFirma = Path.Combine(carpetaFacturador, "FIRMA", year, month);
        string carpetaRpta = Path.Combine(carpetaFacturador, "RPTA", year, month);
        if (!Directory.Exists(carpetaFacturador)) Directory.CreateDirectory(carpetaFacturador);
        if (!Directory.Exists(carpetaData)) Directory.CreateDirectory(carpetaData);
        if (!Directory.Exists(carpetaEnvio)) Directory.CreateDirectory(carpetaEnvio);
        if (!Directory.Exists(carpetaFirma)) Directory.CreateDirectory(carpetaFirma);
        if (!Directory.Exists(carpetaRpta)) Directory.CreateDirectory(carpetaRpta);
    }

    public static void MoverArchivosControl(string storagePath, string sunatArchivos, string nomArch, string year,
        string month)
    {
        string carpetaSfs = Path.Combine(sunatArchivos, "sfs");
        string carpetaFacturador = Path.Combine(storagePath, "facturador");
        string carpetaDestData = Path.Combine(carpetaFacturador, "DATA", year, month);
        string carpetaDestEnvio = Path.Combine(carpetaFacturador, "ENVIO", year, month);
        string carpetaDestFirma = Path.Combine(carpetaFacturador, "FIRMA", year, month);
        string carpetaDestRpta = Path.Combine(carpetaFacturador, "RPTA", year, month);
        string archivoDataOrigen = Path.Combine(carpetaSfs, "DATA", $"{nomArch}.json");
        string archivoEnvioOrigen = Path.Combine(carpetaSfs, "ENVIO", $"{nomArch}.zip");
        string archivoFirmaOrigen = Path.Combine(carpetaSfs, "FIRMA", $"{nomArch}.xml");
        string archivoRptaOrigen = Path.Combine(carpetaSfs, "RPTA", $"R{nomArch}.zip");
        if (File.Exists(archivoDataOrigen))
            File.Move(archivoDataOrigen, Path.Combine(carpetaDestData, $"{nomArch}.json"));
        if (File.Exists(archivoEnvioOrigen))
            File.Move(archivoEnvioOrigen, Path.Combine(carpetaDestEnvio, $"{nomArch}.zip"));
        if (File.Exists(archivoFirmaOrigen))
            File.Move(archivoFirmaOrigen, Path.Combine(carpetaDestFirma, $"{nomArch}.xml"));
        if (File.Exists(archivoRptaOrigen))
            File.Move(archivoRptaOrigen, Path.Combine(carpetaDestRpta, $"R{nomArch}.zip"));
    }

    public static void BorrarArchivosTemporales(string sunatArchivos, string nomArch)
    {
        string carpetaSfs = Path.Combine(sunatArchivos, "sfs");
        string archivoOriDat = Path.Combine(carpetaSfs, "ORIDAT", $"{nomArch}.xml");
        string archivoParse = Path.Combine(carpetaSfs, "PARSE", $"{nomArch}.xml");
        string archivoTemp = Path.Combine(carpetaSfs, "TEMP", $"{nomArch}.xml");
        if (File.Exists(archivoOriDat)) File.Delete(archivoOriDat);
        if (File.Exists(archivoParse)) File.Delete(archivoParse);
        if (File.Exists(archivoTemp)) File.Delete(archivoTemp);
    }

    public static void BorrarTodosLosArchivos(string sunatArchivos, string nomArch)
    {
        BorrarArchivosTemporales(sunatArchivos, nomArch);
        string carpetaSfs = Path.Combine(sunatArchivos, "sfs");
        string archivoData = Path.Combine(carpetaSfs, "DATA", $"{nomArch}.json");
        string archivoEnvio = Path.Combine(carpetaSfs, "ENVIO", $"{nomArch}.zip");
        string archivoFirma = Path.Combine(carpetaSfs, "FIRMA", $"{nomArch}.xml");
        string archivoRpta = Path.Combine(carpetaSfs, "RPTA", $"R{nomArch}.zip");
        if (File.Exists(archivoData)) File.Delete(archivoData);
        if (File.Exists(archivoEnvio)) File.Delete(archivoEnvio);
        if (File.Exists(archivoFirma)) File.Delete(archivoFirma);
        if (File.Exists(archivoRpta)) File.Exists(archivoRpta);
    }
}
