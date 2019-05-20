using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models
{
    public interface IBook
    {
        /// <summary>
        /// Access to property Id
        /// </summary>
        /// <returns>Book.Id</returns>
        int GetId();
        /// <summary>
        /// Access to property name
        /// </summary>
        /// <returns>Book.Name</returns>
        string GetName();
        /// <summary>
        /// Access to property author
        /// </summary>
        /// <returns>Book.Author</returns>
        string GetAuthor();

        /// <summary>
        /// Access to borrowed
        /// </summary>
        /// <returns>instance of class that implements IBorrowed interface</returns>
        IBorrowed GetBorrowed();
        /// <summary>
        /// Check whenever is book borrowed or not
        /// </summary>
        /// <returns>bool</returns>
        bool IsBorrowed();
    }
}
