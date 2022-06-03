using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IAdminUserService : IBaseCrudService<User, UserOutDto, UserAdminCreateDto, UserAdminUpdateDto> {
        Task<UserOutDto[]> GetAllAsync(string search);
        Task<UserShortOutDto[]> GetAllShortAsync(string search);
        Task EnableUserAsync(long id);
        Task DisableUserAsync(long id);
        Task DropPasswordUserAsync(long id);
    }
}
