using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class DirectoryRepository : BaseCrudRepository<Directory>, IDirectoryRepository {
        public DirectoryRepository(IMainDbContext dbContext) : base(dbContext) {
        }
    }
}
