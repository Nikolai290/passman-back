using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using passman_back.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class DirectoryService
        : BaseCrudService<
            Directory,
            DirectoryOutDto,
            DirectoryCreateDto,
            DirectoryUpdateDto
        >, IDirectoryService {

        private readonly IDirectoryRepository directoryRepository;
        public DirectoryService(
            IDirectoryRepository directoryRepository,
            IMapper mapper,
            IValidator<DirectoryCreateDto> createValidator,
            IValidator<DirectoryUpdateDto> updateValidator,
            IHttpContextAccessor accessor,
            IUserRepository userRepository,
            IValidator<User> userValidator
        ) : base(directoryRepository, mapper, createValidator, updateValidator, userValidator, userRepository, accessor) {
            this.directoryRepository = directoryRepository;
        }
        public override async Task<IList<DirectoryOutDto>> GetAllAsync() {
            var user = await GetCurrentUserAndValidateAsync();
            await userValidator.ValidateAndThrowAsync(user);

            if (user.IsAdmin) {
                return await GetTotallyAllAsync();
            }
            var relations = user.GetHashSetRelations();

            var fullAccessDirs = relations
                .Where(rel => rel.Permission == Permission.FullAccess && !rel.IsDeleted)
                .Select(x => x.Directory)
                .Where(x => !x.IsDeleted);

            var readOnlyDirs = relations
                .Where(rel => rel.Permission == Permission.ReadOnly && !rel.IsDeleted)
                .Select(x => x.Directory)
                .Where(x => !x.IsDeleted);

            var fullAccessDirOutDtos = mapper.Map<List<DirectoryOutDto>>(fullAccessDirs);
            var readOnlyDirOutDtos = mapper.Map<List<DirectoryOutDto>>(readOnlyDirs);

            fullAccessDirOutDtos.ForEach(x => x.Permission = Permission.FullAccess);
            readOnlyDirOutDtos.ForEach(x => x.Permission = Permission.ReadOnly);

            var result = new List<DirectoryOutDto>();
            result.AddRange(readOnlyDirOutDtos);
            result.AddRange(fullAccessDirOutDtos);

            var allIds = result.Select(x => x.Id);
            result = result.Where(x => !allIds.Contains(x.ParentId)).ToList();
            foreach (var dto in result) {
                dto.ExcludeChildrenWhoHasNotInList(allIds);
                dto.SetPermissionForAllChildren(relations);
            }

            return result;
        }

        public async Task<IList<DirectoryShortOutDto>> GetAllShortAsync() {
            var user = await GetCurrentUserAndValidateAsync();
            await userValidator.ValidateAndThrowAsync(user);
            if (user.IsAdmin) {
                return await GetTotallyAllShortAsync();
            }
            var relations = user.GetHashSetRelations().Where(rel => !rel.IsDeleted);
            var allDirectories = relations.Select(x => x.Directory).Where(x => !x.IsDeleted);
            var shortOutDtos = mapper.Map<IList<DirectoryShortOutDto>>(allDirectories);
            foreach (var dto in shortOutDtos) {
                dto.Permission = relations.First(x => x.Directory.Id == dto.Id).Permission;
            }
            return shortOutDtos;
        }

        public override async Task<DirectoryOutDto> CreateAsync(DirectoryCreateDto createDto) {
            var user = await GetCurrentUserAndValidateAsync();
            await createValidator.ValidateAndThrowAsync(createDto);
            await userValidator.ValidateAndThrowAsync(user);
            if (!user.HasFullAccess(createDto.ParentId)) {
                throw new UnauthorizedAccessException();
            }

            var directory = new Directory() { Name = createDto.Name };
            if (createDto.ParentId > 0) {
                var parent = await directoryRepository.GetByIdAsync(createDto.ParentId);
                CheckParentContainsSameChildrenAndThrow(parent, createDto.Name);
                directory.Parent = parent;
            }

            var result = await directoryRepository.CreateAsync(directory);

            if (!user.IsAdmin) {
                var userGroup = user.GetHashSetRelations().First(x => x.Directory.Id == createDto.ParentId).UserGroup;
                userGroup.Relations.Add(
                        new UserGroupDirectoryRelation() {
                            UserGroup = userGroup,
                            Directory = result,
                            Permission = Permission.FullAccess
                        });
                await userRepository.UpdateAsync(user);
            }

            var outDto = mapper.Map<DirectoryOutDto>(result);
            return outDto;
        }

        public override async Task<DirectoryOutDto> UpdateAsync(DirectoryUpdateDto updateDto) {
            var user = await GetCurrentUserAndValidateAsync();
            await updateValidator.ValidateAndThrowAsync(updateDto);
            await userValidator.ValidateAndThrowAsync(user);
            if (!user.HasFullAccess(updateDto.Id)) {
                throw new UnauthorizedAccessException();
            }

            var directory = await directoryRepository.GetByIdAsync(updateDto.Id);
            CheckParentContainsSameChildrenAndThrow(directory?.Parent, updateDto.Name);
            mapper.Map(updateDto, directory);

            await directoryRepository.UpdateAsync(directory);
            var outDto = mapper.Map<DirectoryOutDto>(directory);
            return outDto;
        }

        public async Task<DirectoryOutDto> MoveAsync(long id, long parentId) {
            var user = await GetCurrentUserAndValidateAsync();
            await userValidator.ValidateAndThrowAsync(user);
            if (!user.IsAdmin
                && !user.HasFullAccess(id)
                && !(parentId == -1 || user.HasFullAccess(parentId))
            ) {
                throw new UnauthorizedAccessException();
            }

            var directory = await directoryRepository.GetByIdAsync(id);
            if (parentId == -1) {
                directory?.Parent?.Childrens?.Remove(directory);
                directory.Parent = null;
            } else if (directory?.Parent is null || parentId > 0 && directory?.Parent?.Id != parentId) {
                var newParent = await directoryRepository.GetByIdAsync(parentId);
                if (newParent.IsMyParent(directory)) {
                    throw new Exception("Нельзя перемещать родительскую папку в дочернюю");
                }
                CheckParentContainsSameChildrenAndThrow(newParent, directory.Name);
                directory.Parent = newParent;
            }

            await directoryRepository.UpdateAsync(directory);
            var outDto = mapper.Map<DirectoryOutDto>(directory);
            return outDto;
        }

        public override async Task DeleteAsync(long id) {
            var user = await GetCurrentUserAndValidateAsync();
            await userValidator.ValidateAndThrowAsync(user);
            if (!user.HasFullAccess(id)) {
                throw new UnauthorizedAccessException();
            }
            var dir = await directoryRepository.GetByIdAsync(id);
            dir.SetDeleteAllChildrens();
            dir.Parent?.Childrens?.Remove(dir);
            dir.Parent = null;

            await directoryRepository.UpdateAsync(dir);
        }

        private void CheckParentContainsSameChildrenAndThrow(Directory parent, string name) {
            if (parent is null) return;
            if (parent.HasChildrenWithName(name)) {
                throw new Exception("Папка с таким именем уже существует");
            }
        }

        private async Task<DirectoryOutDto[]> GetTotallyAllAsync() {
            var allDirectories = await directoryRepository.GetAllAsync();
            var headDirectories = allDirectories.Where(dir => dir.Parent is null);
            var outDtos = mapper.Map<DirectoryOutDto[]>(headDirectories);
            foreach (var dto in outDtos) {
                dto.SetPermissionForAllChildren(Permission.FullAccess);
            }
            return outDtos;
        }

        private async Task<IList<DirectoryShortOutDto>> GetTotallyAllShortAsync() {
            var allDirectories = await directoryRepository.GetAllAsync();
            var outDtos = mapper.Map<IList<DirectoryShortOutDto>>(allDirectories);
            foreach (var dto in outDtos) {
                dto.Permission = Permission.FullAccess;
            }
            return outDtos;
        }
    }
}
