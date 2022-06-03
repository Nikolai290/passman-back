using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IAdminUserGroupsService : IBaseCrudService<UserGroup, UserGroupOutDto, UserGroupCreateDto, UserGroupUpdateDto> {
        Task<UserGroupOutDto[]> GetAllAsync(string search);
        Task<UserGroupMatrixOutDto[]> GetAllUserGroupMatrixAsync();
        Task MutateUserGroupAsync(long userGroupId, long directoryId, Permission permission);
    }
}
