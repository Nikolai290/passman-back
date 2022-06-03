using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using passman_back.Infrastructure.Business.Extensions;
using SearchingLibrary.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class AdminUserService : BaseCrudService<User, UserOutDto, UserAdminCreateDto, UserAdminUpdateDto>, IAdminUserService {
        private readonly ICrypter crypter;
        private readonly IMailer mailer;
        private readonly IUserGroupsRepository userGroupsRepository;
        private readonly ISearcher searcher;
        public AdminUserService(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<UserAdminCreateDto> createValidator,
            IValidator<UserAdminUpdateDto> updateValidator,
            IValidator<User> userValidator,
            ICrypter crypter,
            IMailer mailer,
            IUserGroupsRepository userGroupsRepository,
            ISearcher searcher,
            IHttpContextAccessor accessor
        ) : base(userRepository, mapper, createValidator, updateValidator, userValidator, userRepository, accessor) {
            this.crypter = crypter;
            this.mailer = mailer;
            this.userGroupsRepository = userGroupsRepository;
            this.searcher = searcher;
        }

        public override async Task<UserOutDto> CreateAsync(UserAdminCreateDto createDto) {
            await GetCurrentUserAndValidateAsync();
            await createValidator.ValidateAndThrowAsync(createDto);
            var user = mapper.Map<User>(createDto);

            user.UserGroups = await userGroupsRepository.GetByIdsAsync(createDto.UserGroupIds);

            string password = GeneratePassword();
            user.Password = crypter.Encrypt(password);

            var createdUser = await userRepository.CreateAsync(user);
            try {
                await mailer.SendMessageNewUserPasswordFromAdmin(user.Email, user.Nickname, password);
            } catch { }

            var outDto = mapper.Map<UserOutDto>(createdUser);
            return outDto;
        }

        private string GeneratePassword() {
            var rnd = new Random();
            var num1 = rnd.Next(100000, 999999);
            var num2 = rnd.Next(100000, 999999);
            var num3 = num1 ^ num2;
            var hex = num3.ToString("X") + num1.ToString("X") + num2.ToString("X");
            return hex;
        }

        public override async Task DeleteAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var user = await userRepository.GetByIdAsync(id);
            user.UserGroups.Clear();
            await userRepository.DeleteAsync(id);
        }

        public async Task DisableUserAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var user = await userRepository.GetByIdAsync(id);
            user.IsBlocked = true;
            await userRepository.UpdateAsync(user);
        }

        public async Task DropPasswordUserAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var user = await userRepository.GetByIdAsync(id);
            var droppedPass = GeneratePassword();
            user.Password = crypter.Encrypt(droppedPass);
            await userRepository.UpdateAsync(user);
            await mailer.SendMessageDropPasswordFromAdmin(user.Email, user.Nickname, droppedPass);
        }

        public async Task EnableUserAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var user = await userRepository.GetByIdAsync(id);
            user.IsBlocked = false;
            await userRepository.UpdateAsync(user);
        }

        public override async Task<IList<UserOutDto>> GetAllAsync() {
            await GetCurrentUserAndValidateAsync();
            var users = await userRepository.GetAllAsync();
            var outDtos = mapper.Map<IList<UserOutDto>>(users);
            return outDtos;
        }

        public async Task<UserOutDto[]> GetAllAsync(string search) {
            await GetCurrentUserAndValidateAsync();
            var users = await userRepository.GetAllAsync();
            var result = users.SearchAnd(search, mapper, searcher);
            var outDtos = mapper.Map<UserOutDto[]>(result);
            return outDtos;
        }

        public async Task<UserShortOutDto[]> GetAllShortAsync(string search) {
            await GetCurrentUserAndValidateAsync();
            var users = await userRepository.GetAllAsync();
            var result = users.SearchAnd(search, mapper, searcher);
            var outDtos = mapper.Map<UserShortOutDto[]>(result);
            return outDtos;
        }

        public override async Task<UserOutDto> GetByIdAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var user = await userRepository.GetByIdAsync(id);
            var outDto = mapper.Map<UserOutDto>(user);
            return outDto;
        }

        public override async Task<UserOutDto> UpdateAsync(UserAdminUpdateDto updateDto) {
            await GetCurrentUserAndValidateAsync();
            await updateValidator.ValidateAndThrowAsync(updateDto);
            var user = await userRepository.GetByIdAsync(updateDto.Id);
            mapper.Map(updateDto, user);
            var newUserGroups = await userGroupsRepository.GetByIdsAsync(updateDto.UserGroupIds);
            user.UserGroups = user.UserGroups.Restruct(newUserGroups);
            await userRepository.UpdateAsync(user);
            var outDto = mapper.Map<UserOutDto>(user);
            return outDto;
        }
    }
}
