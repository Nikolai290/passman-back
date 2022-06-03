using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class InviteCodeRepository : BaseCrudRepository<InviteCode>, IInviteCodeRepository {
        public InviteCodeRepository(IMainDbContext dbContext) : base(dbContext) {
        }

        public async Task<InviteCode> GetByCodeAsync(string code) {
            return await dbContext.InviteCodes.SingleAsync(x => x.Code.Equals(code) && !x.IsDeleted);
        }
    }
}
