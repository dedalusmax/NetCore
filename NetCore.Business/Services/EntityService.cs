using NetCore.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetCore.Business.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace NetCore.Business.Services
{
    public abstract class EntityService<TEntity, TModel, TModelData> : IEntityService<TModel, TModelData>
        where TEntity : class, new()
        where TModel : class
        where TModelData : class
    {
        protected readonly IDatabaseScope _database;
        protected readonly IGenericRepository<TEntity> _entityRepository;

        protected EntityService(IDatabaseScope database, IGenericRepository<TEntity> entityRepository)
        {
            _database = database;
            _entityRepository = entityRepository;
        }

        #region IEntityService
        public async virtual Task<List<TModel>> GetAllAsync()
        {
            return await GetAllEntities(_entityRepository.AsReadOnly())
                .ProjectTo<TModel>()
                .ToListAsync();
        }

        public async virtual Task<TModel> GetAsync(long id)
        {
            var entity = await GetEntityByIdAsync(_entityRepository.AsReadOnly(), id);
            entity.RejectNotFound();
            return Mapper.Map<TModel>(entity);
        }

        public async virtual Task<TModel> CreateAsync(TModelData model)
        {
            model.RejectInvalid();

            var entity = new TEntity();
            UpdateEntity(entity, model);

            _entityRepository.Insert(entity);
            await _database.SaveAsync();

            return Mapper.Map<TModel>(entity);
        }

        public async virtual Task<TModel> UpdateAsync(long id, TModelData model)
        {
            model.RejectInvalid();

            var entity = await GetEntityByIdAsync(_entityRepository.GetAll(), id);
            entity.RejectNotFound();

            UpdateEntity(entity, model);
            await _database.SaveAsync();

            return Mapper.Map<TModel>(entity);
        }

        public async virtual Task DeleteAsync(long id)
        {
            var entity = await GetEntityByIdAsync(_entityRepository.GetAll(), id);
            entity.RejectNotFound();

            _entityRepository.Delete(entity);
            await _database.SaveAsync();
        }

        #endregion

        #region Abstract methods
        protected abstract void UpdateEntity(TEntity entity, TModelData model);
        protected abstract IQueryable<TEntity> GetAllEntities(IQueryable<TEntity> query);
        protected abstract Task<TEntity> GetEntityByIdAsync(IQueryable<TEntity> query, long id);
        #endregion
    }
}
