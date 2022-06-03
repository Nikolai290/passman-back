using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class RestorePasswordCodeRepository : IRestorePasswordCodeRepository {
        private readonly IMainDbContext dbContext;
        public RestorePasswordCodeRepository(IMainDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task<RestorePasswordCode> CreateAsync(RestorePasswordCode code) {
            var result = await dbContext.RestorePasswordCodes.AddAsync(code);
            return result.Entity;
        }

        public async Task DeleteDieCodesAsync() {
            var oldCodes = await dbContext.RestorePasswordCodes
                .Where(x => DateTime.UtcNow > x.AliveBefore)
                .ToListAsync();
            dbContext.RestorePasswordCodes.RemoveRange(oldCodes);
        }

        public void DeleteTotal(RestorePasswordCode code) {
            dbContext.RestorePasswordCodes.Remove(code);
        }

        public async Task<IList<RestorePasswordCode>> GetAllAsync() {
            return await dbContext.RestorePasswordCodes
                .Where(x => DateTime.UtcNow > x.AliveBefore)
                .ToListAsync();
        }

        public async Task<RestorePasswordCode> GetCodeAsync(string code) {
            return await dbContext.RestorePasswordCodes.SingleAsync(x => x.RestoreCode.Equals(code));
        }
    }
}
