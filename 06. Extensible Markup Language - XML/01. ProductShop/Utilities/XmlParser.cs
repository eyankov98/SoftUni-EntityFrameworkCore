using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Utilities
{
    public class XmlParser
    {
        public T[] DeserializeCollection<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]), xmlRootAttribute);

            using StringReader stringReader = new StringReader(inputXml);
            T[] deserializedObj = (T[])xmlSerializer.Deserialize(stringReader);

            return deserializedObj;
        }

        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringReader stringReader = new StringReader(inputXml);
            T deserializedObj = (T)xmlSerializer.Deserialize(stringReader);

            return deserializedObj;
        }

        public string SerializeCollection<T>(T[] obj, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]), xmlRootAttribute);

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            StringBuilder stringBuilder = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(stringBuilder);

            xmlSerializer.Serialize(stringWriter, obj, xmlSerializerNamespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public string Serialize<T>(T obj, string rootName)
        {
            XmlRootAttribute xmlRootAttribute= new XmlRootAttribute(rootName);
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
