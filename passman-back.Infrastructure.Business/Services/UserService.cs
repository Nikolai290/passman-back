using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using passman_back.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class UserService : BaseCrudService<User, UserOutDto, UserRegisterDto, UserUpdateDto>, IUserService {
        private readonly IRestorePasswordCodeRepository restorePasswordCodeRepository;
        private readonly IInviteCodeRepository inviteCodeRepository;
        private readonly IValidator<UserChangePasswordDto> changePasswordValidator;
        private readonly IValidator<UserRestorePasswordStepTwoDto> userRestorePasswordValidator;
        private readonly ICrypter crypter;
        private readonly IMailer mailer;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<UserRegisterDto> createValidator,
            IValidator<UserUpdateDto> updateValidator,
            IValidator<UserChangePasswordDto> changePasswordValidator,
            IValidator<UserRestorePasswordStepTwoDto> userRestorePasswordValidator,
            IValidator<User> userValidator,
            ICrypter crypter,
            IMailer mailer,
            IHttpContextAccessor accessor,
            IRestorePasswordCodeRepository restorePasswordCodeRepository,
            IInviteCodeRepository inviteCodeRepository
        ) : base(userRepository, mapper, createValidator, updateValidator, userValidator, userRepository, accessor) {
            this.restorePasswordCodeRepository = restorePasswordCodeRepository;
            this.crypter = crypter;
            this.mailer = mailer;
            this.changePasswordValidator = changePasswordValidator;
            this.userRestorePasswordValidator = userRestorePasswordValidator;
            this.inviteCodeRepository = inviteCodeRepository;
        }

        public override async Task<UserOutDto> CreateAsync(UserRegisterDto createDto) {
            throw new NotImplementedException();
        }

        public async Task ChangePasswordAsync(UserChangePasswordDto changePasswordDto) {
            var login = httpContext.User.Identity.Name;
            await changePasswordValidator.ValidateAndThrowAsync(changePasswordDto);
            var user = await userRepository.GetByLoginAsync(login);
            AuthenticateOrThrowException(user, changePasswordDto.OldPassword);
            user.Password = crypter.Encrypt(changePasswordDto.NewPassword);
            await userRepository.UpdateAsync(user);
        }

        public async Task<UserOutDto> LoginAsync(UserLoginDto loginDto) {
            if (await FirstAdminCheck(loginDto)) {
                return new UserOutDto() {
                    Nickname = "admin",
                    Role = "admin"
                };
            }
            var user = await userRepository.GetByLoginAsync(loginDto.Login);
            await userValidator.ValidateAndThrowAsync(user);
            AuthenticateOrThrowException(user, loginDto.Password);
            var outDto = mapper.Map<UserOutDto>(user);
            return outDto;
        }

        private async Task<bool> FirstAdminCheck(UserLoginDto loginDto) {
            if (loginDto.Login == "admin" && loginDto.Password == "admin") {
                var users = await userRepository.GetAllAsync();
                if (users.Any(x => x.IsAdmin || x.Nickname == "admin")) {
                    return false;
                }
                var admin = new User() {
                    Nickname = "admin",
                    Role = Role.Admin,
                    Password = crypter.Encrypt("admin"),
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@domain.com"
                };
                await userRepository.CreateAsync(admin);

                return true;
            }
            return false;
        }

        public async Task<UserOutDto> GetByLoginAsync(string login) {
            var user = await userRepository.GetByLoginAsync(login);
            var outDto = mapper.Map<UserOutDto>(user);
            return outDto;
        }

        public override async Task<UserOutDto> UpdateAsync(UserUpdateDto updateDto) {
            var user = await GetCurrentUserAndValidateAsync();
            updateDto.SetId(user.Id);
            await updateValidator.ValidateAndThrowAsync(updateDto);
            mapper.Map(updateDto, user);
            await userRepository.UpdateAsync(user);
            var outDto = mapper.Map<UserOutDto>(user);
            return outDto;
        }

        public async Task StartRestorePasswordAsync(string login) {
            var user = await userRepository.GetByLoginAsync(login);
            await userValidator.ValidateAndThrowAsync(user);
            user.RestorePasswordCode = await GenereateRestorePasswordCodeAsync(user);
            await userRepository.UpdateAsync(user);
            await mailer.SendMessageDropPasswordFromUser(user.Email, user.Nickname, user.RestorePasswordCode.RestoreCode);
        }

        public async Task EndRestorePasswordAsync(UserRestorePasswordStepTwoDto restoreDto) {
            await userRestorePasswordValidator.ValidateAndThrowAsync(restoreDto);
            var code = await restorePasswordCodeRepository.GetCodeAsync(restoreDto.RestorePasswordCode);
            var user = code.User;
            await CheckUserIsBlocked(user);
            user.RestorePasswordCode = null;
            user.Password = crypter.Encrypt(restoreDto.NewPassword);

            await userRepository.UpdateAsync(user);

            await mailer.SendMessagePasswordHasBeenChanged(user.Email, user.Nickname, restoreDto.NewPassword);

            restorePasswordCodeRepository.DeleteTotal(code);
        }

        public async Task CheckRestoreCode(string code) {
            await restorePasswordCodeRepository.DeleteDieCodesAsync();
            await restorePasswordCodeRepository.GetCodeAsync(code);
        }

        public async Task<bool> CheckInviteCodeAsync(string code) {
            var invite = await inviteCodeRepository.GetByCodeAsync(code);
            return invite.isActive;
        }

        private async Task<RestorePasswordCode> GenereateRestorePasswordCodeAsync(User user) {
            if (user.RestorePasswordCode != null) {
                DropOldCode(user);
            }
            var codes = await restorePasswordCodeRepository.GetAllAsync();
            var rnd = new Random();

            var num1 = rnd.Next(100000, 999999);
            var num2 = rnd.Next(100000, 999999);

            var droppedPass = num1 ^ num2;
            while (codes.Any(x => x.RestoreCode.Equals(droppedPass))) {
                droppedPass = rnd.Next(100000, 999999) ^ rnd.Next(100000, 999999);
            }

            var code = new RestorePasswordCode() {
                User = user,
                RestoreCode = droppedPass.ToString(),
                AliveBefore = DateTime.UtcNow.AddHours(1),
            };

            return code;
        }

        private void DropOldCode(User user) {
            var oldCode = user.RestorePasswordCode;
            user.RestorePasswordCode = null;
            oldCode.User = null;
            restorePasswordCodeRepository.DeleteTotal(oldCode);
        }

        private void AuthenticateOrThrowException(User user, string tryPassword) {
            var authenticateSuccess = crypter.Decrypt(user.Password).Equals(tryPassword);
            if (!authenticateSuccess) {
                throw new Exception("Неверный пароль");
            }
        }

        public async Task<UserOutDto> RegisterAsync(UserRegisterDto registerDto, string inviteCode) {
            await createValidator.ValidateAndThrowAsync(registerDto);
            var user = mapper.Map<User>(registerDto);

            try {
                var invite = await inviteCodeRepository.GetByCodeAsync(inviteCode);
                if (!invite.isActive) {
                    throw new Exception("Инвайт код приостановлен или просрочен");
                }
                user.Role = invite.Role;
                user.UserGroups = invite.UserGroups;
            } catch (InvalidOperationException e) {
                throw new InvalidOperationException("Инвайт код не найден");
            }

            var result = await userRepository.CreateAsync(user);
            try {
                await mailer.SendMessageSuccessfullRegistration(user.Email, user.Nickname, registerDto.Password);
            } catch { }

            var outDto = mapper.Map<UserOutDto>(result);
            return outDto;
        }
    }
}
