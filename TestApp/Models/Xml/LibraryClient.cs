using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TestApp.Models.Xml
{
    public class LibraryClient : IClient
    {
        private Library Library;
        public Task<List<IBook>> GetBooks() => Task.Run(() => this.Library.GetBooks());
        public Task SaveAsync() => Task.Run(() => this.Save());

        private readonly string FileName;

        public LibraryClient (string fileName)
        {
            this.FileName = fileName;
            this.Load();
        } 

        private void Load ()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            XmlSerializer serializer = new XmlSerializer(typeof(Library));

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

        }
    }
}
