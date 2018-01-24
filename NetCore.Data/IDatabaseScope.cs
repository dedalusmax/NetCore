using System.Threading.Tasks;

namespace NetCore.Data
{
    public interface IDatabaseScope
    {
        /// <summary>
        /// Saves pending changes asynchronously.
        /// </summary>
        Task<int> SaveAsync();
    }
}
