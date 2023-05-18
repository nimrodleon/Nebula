using System.Xml.Serialization;
using Nebula.Modules.Facturador.Constantes;

namespace Nebula.Modules.Facturador.XmlDigest;

[XmlRoot(ElementName = "SignedInfo", Namespace = EspacioNombres.Ds)]
public class SignedInfo
{
    [XmlElement(ElementName = "Reference", Namespace = EspacioNombres.Ds)]
    public Reference Reference { get; set; } = new Reference();
}
