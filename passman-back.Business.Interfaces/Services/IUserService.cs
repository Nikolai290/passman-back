using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IUserService : IBaseCrudService<User, UserOutDto, UserRegisterDto, UserUpdateDto> {
        Task ChangePasswordAsync(UserChangePasswordDto changePasswordDto);
        Task<UserOutDto> LoginAsync(UserLoginDto loginDto);
        Task<UserOutDto> GetByLoginAsync(string login);
        Task<UserOutDto> UpdateAsync(UserUpdateDto updateDto);
        Task StartRestorePasswordAsync(string login);
        Task EndRestorePasswordAsync(UserRestorePasswordStepTwoDto restoreDto);
        Task CheckRestoreCode(string code);
        Task<bool> CheckInviteCodeAsync(string code);
        Task<UserOutDto> RegisterAsync(UserRegisterDto registerDto, string inviteCode);
    }
}
