using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class UserGroupDirectoryRelationRepository : BaseCrudRepository<UserGroupDirectoryRelation>, IUserGroupDirectoryRelationRepository {
        public UserGroupDirectoryRelationRepository(IMainDbContext dbContext) : base(dbContext) {
        }
    }
}
