using System.Xml.Serialization;

namespace Nebula.Modules.Facturador.XmlDigest;

/// <summary>
/// obtener DigestValue de un XML.
/// </summary>
public class LeerDigestValue
{
    /// <summary>
    /// DigestValue de una boleta|factura.
    /// </summary>
    /// <param name="fileName">nombre archivo xml</param>
    /// <returns>string</returns>
    public string GetInvoiceXmlValue(string fileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(InvoiceXml));
        using FileStream fileStream = new FileStream(fileName, FileMode.Open);
        var invoice = (InvoiceXml)serializer.Deserialize(fileStream)!;
        return invoice.UblExtensions.UblExtension.ExtensionContent.Signature.SignedInfo.Reference.DigestValue;
    }

    /// <summary>
    /// DigestValue de una nota de crédito.
    /// </summary>
    /// <param name="fileName">nombre archivo xml</param>
    /// <returns>string</returns>
    public string GetCreditNoteXmlValue(string fileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(CreditNoteXml));
        using FileStream fileStream = new FileStream(fileName, FileMode.Open);
        var creditNote = (CreditNoteXml)serializer.Deserialize(fileStream)!;
        return creditNote.UblExtensions.UblExtension.ExtensionContent.Signature.SignedInfo.Reference.DigestValue;
    }
}
