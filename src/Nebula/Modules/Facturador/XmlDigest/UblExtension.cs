using System.Xml.Serialization;
using Nebula.Modules.Facturador.Constantes;

namespace Nebula.Modules.Facturador.XmlDigest;

[XmlRoot(ElementName = "UBLExtension", Namespace = EspacioNombres.Ext)]
public class UblExtension
{
    [XmlElement(ElementName = "ExtensionContent", Namespace = EspacioNombres.Ext)]
    public ExtensionContent ExtensionContent { get; set; } = new ExtensionContent();
}
