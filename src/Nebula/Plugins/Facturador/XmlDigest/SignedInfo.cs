using System.Xml.Serialization;
using Nebula.Plugins.Facturador.Constantes;

namespace Nebula.Plugins.Facturador.XmlDigest;

[XmlRoot(ElementName = "SignedInfo", Namespace = EspacioNombres.Ds)]
public class SignedInfo
{
    [XmlElement(ElementName = "Reference", Namespace = EspacioNombres.Ds)]
    public Reference Reference { get; set; } = new Reference();
}
