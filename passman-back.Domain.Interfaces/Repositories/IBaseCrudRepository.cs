using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Domain.Interfaces.Repositories {
    public interface IBaseCrudRepository<TEntity> {
        Task<IList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(long id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(long id);
    }
}
