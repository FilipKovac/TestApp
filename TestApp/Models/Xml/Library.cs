using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestApp.Models.Xml
{
    [XmlRoot("Library")]
    public class Library
    {
        [XmlElement("Book")]
        public List<Book> Books;

        public List<IBook> GetBooks() => this.Books?.Cast<IBook>().ToList();
    }
}
