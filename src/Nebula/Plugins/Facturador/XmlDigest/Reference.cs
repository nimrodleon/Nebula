using System.Xml.Serialization;
using Nebula.Plugins.Facturador.Constantes;

namespace Nebula.Plugins.Facturador.XmlDigest;

[XmlRoot(ElementName = "Reference", Namespace = EspacioNombres.Ds)]
public class Reference
{
    [XmlElement(ElementName = "DigestValue", Namespace = EspacioNombres.Ds)]
    public string DigestValue { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "URI")] public string Uri { get; set; } = string.Empty;
}
