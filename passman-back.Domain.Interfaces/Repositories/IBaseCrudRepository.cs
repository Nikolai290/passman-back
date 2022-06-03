using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Domain.Interfaces.Repositories {
    public interface IBaseCrudRepository<TEntity> {
        Task<IList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(long id);
        Task<IList<TEntity>> GetByIdsAsync(IEnumerable<long> ids);
        Task<TEntity> CreateAsync(TEntity entity);
        Task CreateRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Pseudo-Delete
        /// </summary>
        Task DeleteAsync(long id);
        Task DeleteTotalAsync(long id);
    }
}
