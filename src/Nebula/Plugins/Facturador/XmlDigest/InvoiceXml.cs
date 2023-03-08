using System.Xml.Serialization;
using Nebula.Plugins.Facturador.Constantes;

namespace Nebula.Plugins.Facturador.XmlDigest;

[XmlRoot(ElementName = "Invoice", Namespace = EspacioNombres.XmlnsInvoice)]
public class InvoiceXml
{
    [XmlElement(ElementName = "UBLExtensions", Namespace = EspacioNombres.Ext)]
    public UblExtensions UblExtensions { get; set; } = new UblExtensions();

    [XmlAttribute(AttributeName = "xmlns")]
    public string Xmlns { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "cac", Namespace = EspacioNombres.XmlnsDefault)]
    public string Cac { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "cbc", Namespace = EspacioNombres.XmlnsDefault)]
    public string Cbc { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "ccts", Namespace = EspacioNombres.XmlnsDefault)]
    public string Ccts { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "ds", Namespace = EspacioNombres.XmlnsDefault)]
    public string Ds { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "ext", Namespace = EspacioNombres.XmlnsDefault)]
    public string Ext { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "qdt", Namespace = EspacioNombres.XmlnsDefault)]
    public string Qdt { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "udt", Namespace = EspacioNombres.XmlnsDefault)]
    public string Udt { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "xsi", Namespace = EspacioNombres.XmlnsDefault)]
    public string Xsi { get; set; } = string.Empty;
}
