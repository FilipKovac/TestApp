using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models.ViewModel
{
    public class Borrowed : IBorrowed
    {
        [DisplayName("Meno")]
        public string FirstName { get; set; }
        [DisplayName("Priezvisko")]
        public string LastName { get; set; }
        [DisplayName("Pozicane od")]
        public string From { get; set; }

        public string GetFirstName() => this.FirstName;
        public string GetLastName() => this.LastName;
        public string GetFrom() => this.From;

        public bool IsNull() => this.From.Any();
    }
}
