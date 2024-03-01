using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public class XmlParser
    {
        public T[] DeserializeCollection<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]), xmlRootAttribute);

            using StringReader sr = new StringReader(inputXml);
            T[] deserializedObj = (T[])xmlSerializer.Deserialize(sr);

            return deserializedObj;
        }

        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringReader sr = new StringReader(inputXml);
            T deserializedObj = (T)xmlSerializer.Deserialize(sr);

            return deserializedObj;
        }

        public string SerializeCollection<T>(T[] obj, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]), xmlRootAttribute);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw, obj, xmlSerializerNamespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public string Serialize<T>(T obj, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlSerializer.Serialize(sw , obj, xmlSerializerNamespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
