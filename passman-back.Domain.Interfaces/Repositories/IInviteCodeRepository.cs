using passman_back.Domain.Core.DbEntities;
using System.Threading.Tasks;

namespace passman_back.Domain.Interfaces.Repositories {
    public interface IInviteCodeRepository : IBaseCrudRepository<InviteCode> {
        Task<InviteCode> GetByCodeAsync(string code);
    }
}
