using System.Collections.Generic;
using System.Linq;

namespace NetCore.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private AppDbContext _context { get; }

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public void Delete(TEntity item)
        {
            _context.Set<TEntity>().Remove(item);
        }

        public TEntity Insert(TEntity item)
        {
            _context.Set<TEntity>().Add(item);
            return item;
        }

        public void InsertMultiple(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }
    }
}
