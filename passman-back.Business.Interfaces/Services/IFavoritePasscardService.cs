using passman_back.Business.Dtos;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IFavoritePasscardService {
        Task<PasscardOutDto[]> GetAllAsync();
        Task AddAsync(long id);
        Task RemoveAsync(long id);
    }
}