using System.Xml.Serialization;
using Nebula.Plugins.Facturador.Constantes;

namespace Nebula.Plugins.Facturador.XmlDigest;

[XmlRoot(ElementName = "ExtensionContent", Namespace = EspacioNombres.Ext)]
public class ExtensionContent
{
    [XmlElement(ElementName = "Signature", Namespace = EspacioNombres.Ds)]
    public Signature Signature { get; set; } = new Signature();
}
