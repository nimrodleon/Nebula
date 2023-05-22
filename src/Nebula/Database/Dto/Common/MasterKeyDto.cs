using System.Security.Cryptography;
using Nebula.Modules.Auth.Helpers;

namespace Nebula.Database.Dto.Common;

/// <summary>
/// Genera un archivo hash
/// cada vez que se ejecuta el servidor.
/// </summary>
public static class MasterKeyDto
{
    /// <summary>
    /// Crear Archivo Hash.
    /// </summary>
    /// <returns>true|false</returns>
    public static bool WriteHashFile()
    {
        using var sha256 = SHA256.Create();
        // Generar un hash único a partir del momento actual
        byte[] hash = sha256.ComputeHash(BitConverter.GetBytes(DateTime.Now.Ticks));
        string hashBase64 = Convert.ToBase64String(hash);
        File.WriteAllText(SystemFilePath.MasterKey, hashBase64);
        return File.Exists(SystemFilePath.MasterKey);
    }

    /// <summary>
    /// Lee el Archivo Hash,
    /// </summary>
    /// <returns>Contenido del archivo master.txt</returns>
    public static string ReadHashFile() =>
        File.Exists(SystemFilePath.MasterKey) ?
            File.ReadAllText(SystemFilePath.MasterKey) : string.Empty;
}
