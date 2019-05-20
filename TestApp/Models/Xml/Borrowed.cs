using System.Linq;
using System.Xml.Serialization;

namespace TestApp.Models.Xml
{
    public class Borrowed : IBorrowed
    {
        [XmlElement("FirstName")]
        public string FirstName;
        [XmlElement("LastName")]
        public string LastName;
        [XmlElement("From")]
        public string From;

        public Borrowed() { }
        public Borrowed(IBorrowed borrowed)
        {
            if (borrowed != null)
            {
                this.FirstName = borrowed.GetFirstName();
                this.LastName = borrowed.GetLastName();
                this.From = borrowed.GetFrom();
            }
        }

        public string GetFirstName() => this.FirstName;
        public string GetLastName() => this.LastName;
        public string GetFrom() => this.From;

        public bool IsNull() => this.From == null ? true : !this.From.Any();
    }
}
