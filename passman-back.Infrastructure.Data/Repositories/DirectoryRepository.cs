using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class DirectoryRepository : BaseCrudRepository<Directory>, IDirectoryRepository {
        public DirectoryRepository(IMainDbContext dbContext) : base(dbContext) {
        }

        public override async Task<IList<Directory>> GetAllAsync() {
            var directories = await dbContext
                .Directories
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            return directories.Where(x => !x.HasDeletedParent()).ToList();
        }

        public override async Task<IList<Directory>> GetByIdsAsync(IEnumerable<long> ids) {
            var directories = await dbContext
                .Directories
                .Where(x => !x.IsDeleted)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            return directories.Where(x => !x.HasDeletedParent()).ToList();
        }
    }
}
