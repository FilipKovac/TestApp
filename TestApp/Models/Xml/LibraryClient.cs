using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;

namespace TestApp.Models.Xml
{
    /// <summary>
    /// Provides access to books from Xml file
    /// </summary>
    public class LibraryClient : IClient
    {
        private Library Library;

        private readonly string FileName;
        private readonly XmlSerializer serializer;

        /// <summary>
        /// Create instance and load books from file to the memory
        /// </summary>
        /// <param name="fileName">Path to file where are books saved</param>
        public LibraryClient (string fileName)
        {
            this.FileName = fileName;
            this.serializer = new XmlSerializer(typeof(Library));
            this.Load();
        }

        public IQueryable<IBook> GetBooks() => this.Library.GetBooks().AsQueryable();
        public Task SaveChangesAsync() => Task.Run(() => this.Save());
        public Task<IBook> FindAsync(int id) => Task.Run(() => {
            IBook book = this.Library.Books.Where(b => b.Id == id).FirstOrDefault();
            if (book == null)
                Static.Logger.Fatal("Could not find book with id " + id);
            return book;
        });
        public IBook Add(IBook book)
        {
            Book bookToAdd = (Book)book;
            if (bookToAdd.Id == 0)
                bookToAdd.Id = this.Library.Books.Max(b => b.Id) + 1;
            this.Library.Books.Add(bookToAdd);
            return bookToAdd;
        }

        public IBook Remove(IBook book)
        {
            book = this.Library.Books.Where(b => b.Id == book.GetId()).FirstOrDefault();
            if (book == null)
            {
                Static.Logger.Debug("Trying to remove item, that is not listed");
            }
            else
            {
                this.Library.Books.Remove((Book)book);
            }
            return book;
        }

        public IBook Update(IBook book)
        {
            IBook bookToUpdate = this.Library.Books.Where(b => b.Id == book.GetId()).FirstOrDefault();
            if (bookToUpdate == null)
            {
                Static.Logger.Debug("Trying to update item, that is not listed");
            }
            else
            {
                bookToUpdate = book;
            }
            return bookToUpdate;
        }

        private void Load ()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (XmlReader reader = XmlReader.Create(this.FileName))
            {
                try
                {
                    this.Library = (Library)serializer.Deserialize(reader);
                } catch (System.InvalidOperationException ex)
                {
                    Static.Logger.Fatal("Deserialization problem " + ex.GetBaseException(), ex);
                } finally
                {
                    Static.Logger.Info("End of loading from xml");
                }
            }
        }

        private void Save () {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(this.FileName))
                {
                    serializer.Serialize(writer, this.Library);
                    writer.Close();
                    Static.Logger.Info("Xml Library was successfully saved");
                }
            } catch (System.InvalidOperationException ex)
            {
                Static.Logger.Fatal("Could not save library to Xml file", ex);
            } catch (System.ArgumentNullException ex)
            {
                Static.Logger.Fatal("Could not create xml writer to save to library Xml file", ex);
            }
        }
    }
}
