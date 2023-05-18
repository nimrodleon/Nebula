using System.Xml.Serialization;
using Nebula.Modules.Facturador.Constantes;

namespace Nebula.Modules.Facturador.XmlDigest;

[XmlRoot(ElementName = "Signature", Namespace = EspacioNombres.Ds)]
public class Signature
{
    [XmlElement(ElementName = "SignedInfo", Namespace = EspacioNombres.Ds)]
    public SignedInfo SignedInfo { get; set; } = new SignedInfo();

    [XmlAttribute(AttributeName = "Id")] public string Id { get; set; } = string.Empty;
}
