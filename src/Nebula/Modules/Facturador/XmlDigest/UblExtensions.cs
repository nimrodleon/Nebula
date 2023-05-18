using System.Xml.Serialization;
using Nebula.Modules.Facturador.Constantes;

namespace Nebula.Modules.Facturador.XmlDigest;

[XmlRoot(ElementName = "UBLExtensions", Namespace = EspacioNombres.Ext)]
public class UblExtensions
{
    [XmlElement(ElementName = "UBLExtension", Namespace = EspacioNombres.Ext)]
    public UblExtension UblExtension { get; set; } = new UblExtension();
}
