namespace Nebula.Database.Services.Facturador;

public static class FacturadorControl
{
    public static void CrearDirectorioControl(string storagePath, string year, string month)
    {
        string carpetaSunat = Path.Combine(storagePath, "sunat");
        string carpetaData = Path.Combine(carpetaSunat, "DATA", year, month);
        string carpetaEnvio = Path.Combine(carpetaSunat, "ENVIO", year, month);
        string carpetaFirma = Path.Combine(carpetaSunat, "FIRMA", year, month);
        string carpetaRepo = Path.Combine(carpetaSunat, "REPO", year, month);
        string carpetaRpta = Path.Combine(carpetaSunat, "RPTA", year, month);
        if (!Directory.Exists(carpetaSunat)) Directory.CreateDirectory(carpetaSunat);
        if (!Directory.Exists(carpetaData)) Directory.CreateDirectory(carpetaData);
        if (!Directory.Exists(carpetaEnvio)) Directory.CreateDirectory(carpetaEnvio);
        if (!Directory.Exists(carpetaFirma)) Directory.CreateDirectory(carpetaFirma);
        if (!Directory.Exists(carpetaRepo)) Directory.CreateDirectory(carpetaRepo);
        if (!Directory.Exists(carpetaRpta)) Directory.CreateDirectory(carpetaRpta);
    }

    public static void MoverArchivosControl(string storagePath, string sunatArchivos, string nomArch, string year,
        string month)
    {
        string carpetaSFS = Path.Combine(sunatArchivos, "sfs");
        string carpetaSunat = Path.Combine(storagePath, "sunat");
        string carpetaDestData = Path.Combine(carpetaSunat, "DATA", year, month);
        string carpetaDestEnvio = Path.Combine(carpetaSunat, "ENVIO", year, month);
        string carpetaDestFirma = Path.Combine(carpetaSunat, "FIRMA", year, month);
        string carpetaDestRepo = Path.Combine(carpetaSunat, "REPO", year, month);
        string carpetaDestRpta = Path.Combine(carpetaSunat, "RPTA", year, month);
        string archivoDataOrigen = Path.Combine(carpetaSFS, "DATA", $"{nomArch}.json");
        string archivoEnvioOrigen = Path.Combine(carpetaSFS, "ENVIO", $"{nomArch}.zip");
        string archivoFirmaOrigen = Path.Combine(carpetaSFS, "FIRMA", $"{nomArch}.xml");
        string archivoRepoOrigen = Path.Combine(carpetaSFS, "REPO", $"{nomArch}.pdf");
        string archivoRptaOrigen = Path.Combine(carpetaSFS, "RPTA", $"R{nomArch}.zip");
        if (File.Exists(archivoDataOrigen))
            File.Move(archivoDataOrigen, Path.Combine(carpetaDestData, $"{nomArch}.json"));
        if (File.Exists(archivoEnvioOrigen))
            File.Move(archivoEnvioOrigen, Path.Combine(carpetaDestEnvio, $"{nomArch}.zip"));
        if (File.Exists(archivoFirmaOrigen))
            File.Move(archivoFirmaOrigen, Path.Combine(carpetaDestFirma, $"{nomArch}.xml"));
        if (File.Exists(archivoRepoOrigen))
            File.Move(archivoRepoOrigen, Path.Combine(carpetaDestRepo, $"{nomArch}.pdf"));
        if (File.Exists(archivoRptaOrigen))
            File.Move(archivoRptaOrigen, Path.Combine(carpetaDestRpta, $"R{nomArch}.zip"));
    }

    public static void BorrarArchivosTemporales(string sunatArchivos, string nomArch)
    {
        string carpetaSFS = Path.Combine(sunatArchivos, "sfs");
        string archivoOriDat = Path.Combine(carpetaSFS, "ORIDAT", $"{nomArch}.xml");
        string archivoParse = Path.Combine(carpetaSFS, "PARSE", $"{nomArch}.xml");
        string archivoTemp = Path.Combine(carpetaSFS, "TEMP", $"{nomArch}.xml");
        if (File.Exists(archivoOriDat)) File.Delete(archivoOriDat);
        if (File.Exists(archivoParse)) File.Delete(archivoParse);
        if (File.Exists(archivoTemp)) File.Delete(archivoTemp);
    }

    public static void BorrarTodosLosArchivos(string sunatArchivos, string nomArch)
    {
        BorrarArchivosTemporales(sunatArchivos, nomArch);
        string carpetaSFS = Path.Combine(sunatArchivos, "sfs");
        string archivoData = Path.Combine(carpetaSFS, "DATA", $"{nomArch}.json");
        string archivoEnvio = Path.Combine(carpetaSFS, "ENVIO", $"{nomArch}.zip");
        string archivoFirma = Path.Combine(carpetaSFS, "FIRMA", $"{nomArch}.xml");
        string archivoRepo = Path.Combine(carpetaSFS, "REPO", $"{nomArch}.pdf");
        string archivoRpta = Path.Combine(carpetaSFS, "RPTA", $"R{nomArch}.zip");
        if (File.Exists(archivoData)) File.Delete(archivoData);
        if (File.Exists(archivoEnvio)) File.Delete(archivoEnvio);
        if (File.Exists(archivoFirma)) File.Delete(archivoFirma);
        if (File.Exists(archivoRepo)) File.Exists(archivoRepo);
        if (File.Exists(archivoRpta)) File.Exists(archivoRpta);
    }
}
