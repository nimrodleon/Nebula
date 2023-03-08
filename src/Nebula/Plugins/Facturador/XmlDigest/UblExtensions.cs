using System.Xml.Serialization;
using Nebula.Plugins.Facturador.Constantes;

namespace Nebula.Plugins.Facturador.XmlDigest;

[XmlRoot(ElementName = "UBLExtensions", Namespace = EspacioNombres.Ext)]
public class UblExtensions
{
    [XmlElement(ElementName = "UBLExtension", Namespace = EspacioNombres.Ext)]
    public UblExtension UblExtension { get; set; } = new UblExtension();
}
