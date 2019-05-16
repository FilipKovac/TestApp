using System.Xml.Serialization;

namespace TestApp.Models.Xml
{
    public class Book
    {
        [XmlAttribute("id")]
        public int ID;
        [XmlElement("Name")]
        public string Name;
        [XmlElement("Author")]
        public string Author;
        [XmlElement("Borrowed")]
        public Borrowed Borrowed;
    }
}
