using System.Xml.Serialization;

namespace Nebula.Database.Dto.Sales;

public class LeerDigestValue
{
    [XmlRoot(ElementName = "Reference", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public class Reference
    {
        [XmlElement(ElementName = "DigestValue", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public string DigestValue { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "URI")] public string Uri { get; set; } = string.Empty;
    }

    [XmlRoot(ElementName = "SignedInfo", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public class SignedInfo
    {
        [XmlElement(ElementName = "Reference", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Reference Reference { get; set; } = new Reference();
    }

    [XmlRoot(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public class Signature
    {
        [XmlElement(ElementName = "SignedInfo", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public SignedInfo SignedInfo { get; set; } = new SignedInfo();

        [XmlAttribute(AttributeName = "Id")] public string Id { get; set; } = string.Empty;
    }

    [XmlRoot(ElementName = "ExtensionContent",
        Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
    public class ExtensionContent
    {
        [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Signature Signature { get; set; } = new Signature();
    }

    [XmlRoot(ElementName = "UBLExtension",
        Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
    public class UblExtension
    {
        [XmlElement(ElementName = "ExtensionContent",
            Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
        public ExtensionContent ExtensionContent { get; set; } = new ExtensionContent();
    }

    [XmlRoot(ElementName = "UBLExtensions",
        Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
    public class UblExtensions
    {
        [XmlElement(ElementName = "UBLExtension",
            Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
        public UblExtension UblExtension { get; set; } = new UblExtension();
    }

    [XmlRoot(ElementName = "Invoice", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2")]
    public class Invoice
    {
        [XmlElement(ElementName = "UBLExtensions",
            Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
        public UblExtensions UblExtensions { get; set; } = new UblExtensions();

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "cac", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Cac { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "cbc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Cbc { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "ccts", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ccts { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "ds", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ds { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "ext", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ext { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "qdt", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Qdt { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "udt", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Udt { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; } = string.Empty;
    }

    public string GetValue(string fileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Invoice));
        using FileStream fileStream = new FileStream(fileName, FileMode.Open);
        var invoice = (Invoice)serializer.Deserialize(fileStream)!;
        return invoice.UblExtensions.UblExtension.ExtensionContent.Signature.SignedInfo.Reference.DigestValue;
    }
}
