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

        public Book() { }
        public Book(IBook book)
        {
            this.Id = book.GetId();
            this.Name = book.GetName();
            this.Author = book.GetAuthor();
            this.Borrowed = new Borrowed(book.GetBorrowed());
        }

        public string GetAuthor() => this.Author;
        public int GetId() => this.Id;
        public string GetName() => this.Name;
        public IBorrowed GetBorrowed() => this.Borrowed;

        public bool IsBorrowed() => !this.Borrowed.IsNull();
    }
}
