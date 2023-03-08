using System.Xml.Serialization;
using Nebula.Plugins.Facturador.Constantes;

namespace Nebula.Plugins.Facturador.XmlDigest;

[XmlRoot(ElementName = "UBLExtension", Namespace = EspacioNombres.Ext)]
public class UblExtension
{
    [XmlElement(ElementName = "ExtensionContent", Namespace = EspacioNombres.Ext)]
    public ExtensionContent ExtensionContent { get; set; } = new ExtensionContent();
}
