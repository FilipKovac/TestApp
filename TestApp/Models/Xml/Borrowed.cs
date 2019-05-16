using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestApp.Models.Xml
{
    public class Borrowed
    {
        [XmlElement("FirstName")]
        public string FirstName;
        [XmlElement("LastName")]
        public string LastName;
        [XmlElement("From")]
        public string From;
    }
}
