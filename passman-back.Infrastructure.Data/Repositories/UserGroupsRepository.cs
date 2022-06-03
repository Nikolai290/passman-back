using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class UserGroupsRepository : BaseCrudRepository<UserGroup>, IUserGroupsRepository {
        public UserGroupsRepository(IMainDbContext dbContext) : base(dbContext) {
        }

        public override async Task<UserGroup> GetByIdAsync(long id) {
            var entity = await dbContext.UserGroups.Include(x => x.Users)
                .SingleAsync(x => x.Id == id);
            return entity;
        }
    }
}
