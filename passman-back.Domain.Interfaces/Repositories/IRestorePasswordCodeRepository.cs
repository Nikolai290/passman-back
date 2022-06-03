using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Domain.Interfaces.Repositories {
    public interface IRestorePasswordCodeRepository {
        Task<IList<RestorePasswordCode>> GetAllAsync();
        Task<RestorePasswordCode> CreateAsync(RestorePasswordCode restorePasswordCode);
        void DeleteTotal(RestorePasswordCode code);
        Task<RestorePasswordCode> GetCodeAsync(string code);
        Task DeleteDieCodesAsync();
    }
}
