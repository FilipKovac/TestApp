using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models.Xml
{
    public class LibraryClient : IClient
    {
        private List<IBook> Books;
        public Task<List<IBook>> GetBooks() => Task.Run(() => this.Books);

        private readonly string FileName;

        public LibraryClient (string fileName)
        {
            this.FileName = fileName;

            this.Load();
        } 

        private void Load () { }

        private void Save () { }

    }
}
