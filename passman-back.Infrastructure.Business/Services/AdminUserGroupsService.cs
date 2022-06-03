using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using passman_back.Domain.Interfaces.Repositories;
using passman_back.Infrastructure.Business.Extensions;
using SearchingLibrary.Service;
using SearchingLibrary.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class AdminUserGroupsService : BaseCrudService<UserGroup, UserGroupOutDto, UserGroupCreateDto, UserGroupUpdateDto>, IAdminUserGroupsService {
        private readonly IUserGroupsRepository userGroupsRepository;
        private readonly IDirectoryRepository directoryRepository;
        private readonly ISearcher searcher;

        public AdminUserGroupsService(
            IUserGroupsRepository baseCrudRepository,
            IMapper mapper,
            IValidator<UserGroupCreateDto> createValidator,
            IValidator<UserGroupUpdateDto> updateValidator,
            IValidator<User> userValidator,
            ISearcher searcher,
            IDirectoryRepository directoryRepository,
            IUserRepository userRepository,
            IHttpContextAccessor accessor
        ) : base(baseCrudRepository, mapper, createValidator, updateValidator, userValidator, userRepository, accessor) {
            this.userGroupsRepository = baseCrudRepository;
            this.searcher = searcher;
            this.directoryRepository = directoryRepository;
        }
        public override async Task<UserGroupOutDto> CreateAsync(UserGroupCreateDto createDto) {
            await GetCurrentUserAndValidateAsync();
            await createValidator.ValidateAndThrowAsync(createDto);
            var userGroup = mapper.Map<UserGroup>(createDto);
            userGroup.Relations = new List<UserGroupDirectoryRelation>();

            userGroup.Users = await userRepository.GetByIdsAsync(createDto.UserIds);
            var directories = await directoryRepository.GetAllAsync();

            foreach (var relation in createDto.Relations) {
                var directory = directories.First(x => x.Id == relation.DirectoryId);
                AddRelation(userGroup, directory, relation.Permission);
            }

            var result = await baseCrudRepository.CreateAsync(userGroup);
            var outDto = mapper.Map<UserGroupOutDto>(result);
            return outDto;
        }

        public override async Task<UserGroupOutDto> UpdateAsync(UserGroupUpdateDto updateDto) {
            await GetCurrentUserAndValidateAsync();
            await updateValidator.ValidateAndThrowAsync(updateDto);
            var userGroup = await userGroupsRepository.GetByIdAsync(updateDto.Id);
            mapper.Map(updateDto, userGroup);

            var users = await userRepository.GetByIdsAsync(updateDto.UserIds);
            userGroup.UpdateUserList(users);

            await baseCrudRepository.UpdateAsync(userGroup);
            var outDto = mapper.Map<UserGroupOutDto>(userGroup);
            return outDto;
        }

        public override async Task DeleteAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var userGroup = await userGroupsRepository.GetByIdAsync(id);
            foreach (var relation in userGroup.Relations) {
                relation.IsDeleted = true;
            }
            userGroup.Relations.Clear();
            userGroup.Users.Clear();

            await base.DeleteAsync(id);
        }

        public async Task<UserGroupOutDto[]> GetAllAsync(string search) {
            await GetCurrentUserAndValidateAsync();
            var userGroups = await userGroupsRepository.GetAllAsync();
            var result = userGroups.SearchAnd(search, mapper, searcher);
            var outDtos = mapper.Map<UserGroupOutDto[]>(result);
            return outDtos;
        }

        public async Task<UserGroupMatrixOutDto[]> GetAllUserGroupMatrixAsync() {
            await GetCurrentUserAndValidateAsync();
            var userGroups = await userGroupsRepository.GetAllAsync();
            var outDtos = mapper.Map<UserGroupMatrixOutDto[]>(userGroups);
            return outDtos;
        }

        public async Task MutateUserGroupAsync(long userGroupId, long directoryId, Permission permission) {
            await GetCurrentUserAndValidateAsync();
            var userGroup = await userGroupsRepository.GetByIdAsync(userGroupId);
            var directory = await directoryRepository.GetByIdAsync(directoryId);
            ToggleRelation(userGroup, directory, permission);
            await userGroupsRepository.UpdateAsync(userGroup);
        }

        private void ToggleRelation(UserGroup userGroup, Directory directory, Permission permission) {
            var relationExists = userGroup.Relations.FirstOrDefault(rel => rel.Directory.Id ==directory.Id);
            if (relationExists != null) {
                relationExists.Permission = permission;
                return;
            }
            AddRelation(userGroup, directory, permission);
        }

        private void AddRelation(UserGroup userGroup, Directory directory, Permission permission) {
            if (userGroup.Relations.Any(rel => rel.Directory.Id == directory.Id)) {
                return;
            }
            var relation = new UserGroupDirectoryRelation(){
                UserGroup = userGroup,
                Directory = directory,
                Permission = permission
            };
            userGroup.Relations.Add(relation);
            directory.Relations.Add(relation);
        }
    }
}

