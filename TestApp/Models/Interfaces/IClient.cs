using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models
{
    /// <summary>
    /// Interface provides access to data 
    /// Not important the storage type
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Add access to datatable of books
        /// </summary>
        /// <returns>Queryable array of objects that implement IBook interface</returns>
        IQueryable<IBook> GetBooks();
        /// <summary>
        /// Look for a object in set by its "primary key"
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found object or null if not listed</returns>
        Task<IBook> FindAsync(int id);
        /// <summary>
        /// Add object to the set
        /// </summary>
        /// <param name="book">Object to add without Id</param>
        /// <returns>Object with Id assigned to it</returns>
        IBook Add(IBook book);
        /// <summary>
        /// Remove object from set
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Removed object or null if failed</returns>
        IBook Remove(IBook book);
        /// <summary>
        /// Update object in the set
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Updated object</returns>
        IBook Update(IBook book);

        /// <summary>
        /// Save changes to database
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
