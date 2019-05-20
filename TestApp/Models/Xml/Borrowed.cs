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

        public string GetFirstName() => this.FirstName;
        public string GetLastName() => this.LastName;
        public string GetFrom() => this.From;

        public bool IsNull() => this.From.Any();
    }
}
