using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models.ViewModel
{
    /// <summary>
    /// DTO object of Book 
    /// </summary>
    public class Book : IBook
    {
        public int Id { get; set; }
        [DisplayName("Nazov")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Autor")]
        [Required]
        public string Author { get; set; }
        [DisplayName("Pozicane")]
        public Borrowed Borrowed { get; set; }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Book() { }
        /// <summary>
        /// Creates ViewModel from any other model that implements IBook interface
        /// </summary>
        /// <param name="book"></param>
        public Book (IBook book)
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
