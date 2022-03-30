using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;
using passman_back.Infrastructure.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class BaseCrudRepository<TEntity> : IBaseCrudRepository<TEntity>
        where TEntity : AbstractDbEntity {

        protected readonly IMainDbContext dbContext;
        private readonly MainDbContext db;

        public BaseCrudRepository(IMainDbContext dbContext) {
            this.dbContext = dbContext;
            this.db = (MainDbContext)dbContext;
        }
        public virtual async Task<TEntity> CreateAsync(TEntity obj) {
            var result = (await db
                .Set<TEntity>()
                .AddAsync(obj))
                .Entity;
            await db.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// Pseudo-Delete
        /// </summary>
        public virtual async Task DeleteAsync(long id) {
            var entity = await GetByIdAsync(id);
            entity.IsDeleted = true;
            await db.SaveChangesAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllAsync() {
            var entities = await db
                .Set<TEntity>()
                .ToListAsync();
            return entities;
        }

        public virtual async Task<TEntity> GetByIdAsync(long id) {
            var entity = await db
                .Set<TEntity>()
                .SingleAsync(x => !x.IsDeleted && x.Id == id);
            return entity;
        }

        public virtual async Task<IList<TEntity>> GetByIdsAsync(IList<long> ids) {
            if (ids.Count == 0) {
                return new List<TEntity>();
            }
            var entities = await db
                .Set<TEntity>()
                .Where(x => !x.IsDeleted && ids.Contains(x.Id))
                .ToListAsync();
            return entities;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity) {
            var result = db.Set<TEntity>().Update(entity).Entity;
            await db.SaveChangesAsync();
            return result;
        }
    }
}
