using Nebula.Database.Models.Common;

namespace Nebula.Database.Services.Facturador;

public static class FacturadorControl
{
    public static void CrearDirectorioControl(Configuration configuration, string year, string month)
    {
        string carpetaControl = Path.Combine(configuration.FileSunat, "CONTROL");
        string carpetaData = Path.Combine(carpetaControl, "DATA", year, month);
        string carpetaEnvio = Path.Combine(carpetaControl, "ENVIO", year, month);
        string carpetaRepo = Path.Combine(carpetaControl, "REPO", year, month);
        string carpetaRpta = Path.Combine(carpetaControl, "RPTA", year, month);
        if (!Directory.Exists(carpetaControl)) Directory.CreateDirectory(carpetaControl);
        if (!Directory.Exists(carpetaData)) Directory.CreateDirectory(carpetaData);
        if (!Directory.Exists(carpetaEnvio)) Directory.CreateDirectory(carpetaEnvio);
        if (!Directory.Exists(carpetaRepo)) Directory.CreateDirectory(carpetaRepo);
        if (!Directory.Exists(carpetaRpta)) Directory.CreateDirectory(carpetaRpta);
    }

    public static void MoverArchivosControl(Configuration configuration, string nomArch, string year, string month)
    {
        string carpetaSFS = Path.Combine(configuration.FileSunat, "sfs");
        string carpetaControl = Path.Combine(configuration.FileSunat, "CONTROL");
        string carpetaDestData = Path.Combine(carpetaControl, "DATA", year, month);
        string carpetaDestEnvio = Path.Combine(carpetaControl, "ENVIO", year, month);
        string carpetaDestRepo = Path.Combine(carpetaControl, "REPO", year, month);
        string carpetaDestRpta = Path.Combine(carpetaControl, "RPTA", year, month);
        string archivoDataOrigen = Path.Combine(carpetaSFS, "DATA", $"{nomArch}.json");
        string archivoEnvioOrigen = Path.Combine(carpetaSFS, "ENVIO", $"{nomArch}.zip");
        string archivoRepoOrigen = Path.Combine(carpetaSFS, "REPO", $"{nomArch}.pdf");
        string archivoRptaOrigen = Path.Combine(carpetaSFS, "RPTA", $"R{nomArch}.zip");
        if (File.Exists(archivoDataOrigen)) File.Move(archivoDataOrigen, Path.Combine(carpetaDestData, $"{nomArch}.json"));
        if (File.Exists(archivoEnvioOrigen)) File.Move(archivoEnvioOrigen, Path.Combine(carpetaDestEnvio, $"{nomArch}.zip"));
        if (File.Exists(archivoRepoOrigen)) File.Move(archivoRepoOrigen, Path.Combine(carpetaDestRepo, $"{nomArch}.pdf"));
        if (File.Exists(archivoRptaOrigen)) File.Move(archivoRptaOrigen, Path.Combine(carpetaDestRpta, $"R{nomArch}.zip"));
    }

}
