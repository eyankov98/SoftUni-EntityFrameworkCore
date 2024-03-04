using System.Text;
using System.Xml.Serialization;

namespace Boardgames.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringReader stringReader = new StringReader(inputXml);

            T deserializedDtos = (T)xmlSerializer.Deserialize(stringReader);

            return deserializedDtos;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            StringBuilder stringBuilder = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(stringBuilder);

            xmlSerializer.Serialize(stringWriter, obj, xmlSerializerNamespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
