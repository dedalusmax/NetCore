using System.Collections.Generic;
using System.Linq;

namespace NetCore.Data
{
    /// <summary>
    /// Represents repository of items, with functionality to get entities, add new entities and delete existing.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>Query of entities.</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Inserted entity.</returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts the list of specified entities.
        /// </summary>
        /// <param name="entity">The entity list.</param>
        void InsertMultiple(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);
    }
}
