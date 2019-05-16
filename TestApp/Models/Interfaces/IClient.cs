using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models
{
    public interface IClient
    {
        Task<List<IBook>> GetBooks();
        Task SaveAsync();
    }
}
