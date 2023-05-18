using System.Xml.Serialization;
using Nebula.Modules.Facturador.Constantes;

namespace Nebula.Modules.Facturador.XmlDigest;

[XmlRoot(ElementName = "ExtensionContent", Namespace = EspacioNombres.Ext)]
public class ExtensionContent
{
    [XmlElement(ElementName = "Signature", Namespace = EspacioNombres.Ds)]
    public Signature Signature { get; set; } = new Signature();
}
