using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Nebula.Database.Dto.Sales;

public class LeerDigestValue
{
    class Invoice : IXmlSerializable
    {
        public string DigestValue { get; set; } = string.Empty;

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            DigestValue = "-:)";
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public string GetValue(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Invoice));
        using StreamReader reader = new StreamReader(path);
        Invoice invoice = (Invoice)serializer.Deserialize(reader)!;
        return invoice.DigestValue;
    }
}
