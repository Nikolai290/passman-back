using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class PasscardRepository : BaseCrudRepository<Passcard>, IPasscardRepository {
        public PasscardRepository(IMainDbContext dbContext) : base(dbContext) { }
        public override async Task<IList<Passcard>> GetAllAsync() {
            var passcards = await dbContext
                .Passcards
                .Where(x => !x.IsDeleted
                        && x.Parents.Count > 0)
                .ToListAsync();
            return passcards.Where(x => x.Parents.Any(dir => !dir.HasDeletedParent())).ToList();
        }
    }
}
