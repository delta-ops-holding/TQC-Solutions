using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Interfaces
{
    /// <summary>
    /// Contains the base of the CRUD operations for a repository
    /// </summary>
    /// <typeparam name="T">The type to parse with the repository.</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Inserts a new type.
        /// </summary>
        /// <param name="obj">The type of the object to insert.</param>
        public void Insert(T obj);

        /// <summary>
        /// Updates an existing type.
        /// </summary>
        /// <param name="obj">Defines the data to update.</param>
        public void Update(T obj);

        /// <summary>
        /// Deletes a type by id.
        /// </summary>
        /// <param name="id">Used to identify the type to delete.</param>
        public void Delete(int id);

        /// <summary>
        /// Get a type by id.
        /// </summary>
        /// <param name="id">Used to identify the type to get.</param>
        /// <returns>A type.</returns>
        public T GetById(int id);

        /// <summary>
        /// Get all types.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        public IEnumerable<T> GetAll();
    }
}
