using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace Nebula.Modules.InvoiceHub.Helpers;

public class PfxToPemConverter
{
    public static string ConvertPfxToPem(byte[] pfxData, string password)
    {
        X509Certificate2 certificate = new X509Certificate2(pfxData, password, X509KeyStorageFlags.Exportable);
        RSA privateKey = certificate.GetRSAPrivateKey();

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("-----BEGIN CERTIFICATE-----");
        builder.AppendLine(Convert.ToBase64String(certificate.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
        builder.AppendLine("-----END CERTIFICATE-----");
        builder.AppendLine("-----BEGIN PRIVATE KEY-----");
        builder.AppendLine(Convert.ToBase64String(privateKey.ExportRSAPrivateKey(), Base64FormattingOptions.InsertLineBreaks));
        builder.AppendLine("-----END PRIVATE KEY-----");

        return builder.ToString();
    }
}
