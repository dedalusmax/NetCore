using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Business.Services
{
    public interface IEntityService<TModel, TModelData>
         where TModel : class
         where TModelData : class
    {
        Task<List<TModel>> GetAllAsync();
        Task<TModel> GetAsync(long id);
        Task<TModel> CreateAsync(TModelData learningLevel);
        Task<TModel> UpdateAsync(long id, TModelData learningLevel);
        Task DeleteAsync(long id);
    }
}
