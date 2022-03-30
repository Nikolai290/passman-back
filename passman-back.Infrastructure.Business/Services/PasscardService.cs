using AutoMapper;
using FluentValidation;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class PasscardService
        : BaseCrudService<
            Passcard,
            PasscardOutDto,
            PasscardCreateDto,
            PasscardUpdateDto
        >, IPasscardService {

        private readonly IPasscardRepository passcardRepository;
        private readonly IDirectoryRepository directoryRepository;
        public PasscardService(
            IPasscardRepository passcardRepository,
            IMapper mapper,
            IValidator<PasscardCreateDto> createValidator,
            IValidator<PasscardUpdateDto> updateValidator,
            IDirectoryRepository directoryRepository
        ) : base(passcardRepository, mapper, createValidator, updateValidator) {
            this.passcardRepository = passcardRepository;
            this.directoryRepository = directoryRepository;
        }

        public override async Task<PasscardOutDto> CreateAsync(PasscardCreateDto createDto) {
            await createValidator.ValidateAndThrowAsync(createDto);
            var entity = mapper.Map<Passcard>(createDto);
            entity.Parents = await directoryRepository.GetByIdsAsync(createDto.ParentIds);
            var result = await baseCrudRepository.CreateAsync(entity);
            var outDto = mapper.Map<PasscardOutDto>(result);
            return outDto;
        }

        public override async Task<PasscardOutDto> UpdateAsync(long id, PasscardUpdateDto updateDto) {
            await updateValidator.ValidateAndThrowAsync(updateDto);
            var entity = mapper.Map<Passcard>(updateDto);
            // TODO
            entity.Parents = await directoryRepository.GetByIdsAsync(updateDto.ParentIds);
            // ^^^
            var result = await baseCrudRepository.CreateAsync(entity);
            var outDto = mapper.Map<PasscardOutDto>(result);
            return outDto;
        }

    }
}
