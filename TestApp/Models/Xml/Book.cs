using System.Xml.Serialization;

namespace TestApp.Models.Xml
{
    public class Book : IBook
    {
        [XmlAttribute("id")]
        public int Id;
        [XmlElement("Name")]
        public string Name;
        [XmlElement("Author")]
        public string Author;
        [XmlElement("Borrowed")]
        public Borrowed Borrowed;

        public string GetAuthor() => this.Author;
        public int GetId() => this.Id;
        public string GetName() => this.Name;
        public IBorrowed GetBorrowed() => this.Borrowed;

        public bool IsBorrowed() => this.Borrowed.IsNull();
    }
}
