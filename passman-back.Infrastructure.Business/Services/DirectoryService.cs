using AutoMapper;
using FluentValidation;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
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
            IValidator<DirectoryUpdateDto> updateValidator
        ) : base(directoryRepository, mapper, createValidator, updateValidator) {
            this.directoryRepository = directoryRepository;
        }
        public override async Task<IList<DirectoryOutDto>> GetAllAsync() {
            var allDirectories = await directoryRepository.GetAllAsync();
            var headDirectories = allDirectories.Where(dir => dir.Parent is null);
            var outDtos = mapper.Map<IList<DirectoryOutDto>>(headDirectories);
            return outDtos;
        }

        public async Task<IList<DirectoryShortOutDto>> GetAllShortAsync() {
            var allDirectories = await directoryRepository.GetAllAsync();
            var shortOutDtos = mapper.Map<IList<DirectoryShortOutDto>>(allDirectories);
            return shortOutDtos;
        }

        public override async Task<DirectoryOutDto> CreateAsync(DirectoryCreateDto createDto) {
            await createValidator.ValidateAndThrowAsync(createDto);
            var directory = new Directory() { Name = createDto.Name };
            if (createDto.ParentId.HasValue && createDto.ParentId.Value > 0) {
                var parent = await directoryRepository.GetByIdAsync(createDto.ParentId.Value);
                directory.Parent = parent;
            }
            var result = await directoryRepository.CreateAsync(directory);
            var outDto = mapper.Map<DirectoryOutDto>(result);
            return outDto;
        }

        public override async Task<DirectoryOutDto> UpdateAsync(long id, DirectoryUpdateDto updateDto) {
            await updateValidator.ValidateAndThrowAsync(updateDto);
            var directory = await directoryRepository.GetByIdAsync(id);

            if (
                updateDto.ParentId.HasValue
                && directory?.Parent.Id != updateDto.ParentId.Value
                && updateDto.ParentId.Value > 0
            ) {
                var parent = await directoryRepository.GetByIdAsync(updateDto.ParentId.Value);
                directory.Parent = parent;
            }

            mapper.Map(updateDto, directory);

            await directoryRepository.UpdateAsync(directory);
            var outDto = mapper.Map<DirectoryOutDto>(directory);
            return outDto;
        }

        public override async Task DeleteAsync(long id) {
            var dir = await directoryRepository.GetByIdAsync(id);
            dir.Parent.Childrens.Remove(dir);
            dir.Parent = null;
            await base.DeleteAsync(id);
        }
    }
}
