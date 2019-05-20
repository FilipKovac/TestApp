using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models
{
    public interface IBorrowed
    {
        /// <summary>
        /// Access to property FirstName
        /// </summary>
        /// <returns>Borrowed.FirstName</returns>
        string GetFirstName();
        /// <summary>
        /// Access to property LastName
        /// </summary>
        /// <returns>Borrowed.LastName</returns>
        string GetLastName();
        /// <summary>
        /// Access to property From
        /// </summary>
        /// <returns>Borrowed.From</returns>
        string GetFrom();
    }
}
