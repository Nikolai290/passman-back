using AutoMapper;
using FluentValidation;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using System.Collections.Generic;
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
            var passcard = mapper.Map<Passcard>(createDto);
            passcard.Parents = await directoryRepository.GetByIdsAsync(createDto.ParentIds);
            var result = await baseCrudRepository.CreateAsync(passcard);
            var outDto = mapper.Map<PasscardOutDto>(result);
            return outDto;
        }

        public override async Task<PasscardOutDto> UpdateAsync(long id, PasscardUpdateDto updateDto) {
            await updateValidator.ValidateAndThrowAsync(updateDto);
            var passcard = await passcardRepository.GetByIdAsync(id);
            mapper.Map(updateDto, passcard);
            // TODO
            passcard.Parents = await directoryRepository.GetByIdsAsync(updateDto.ParentIds);
            // ^^^
            await baseCrudRepository.UpdateAsync(passcard);
            var outDto = mapper.Map<PasscardOutDto>(passcard);
            return outDto;
        }

        public override async Task DeleteAsync(long id) {
            var pass = await passcardRepository.GetByIdAsync(id);
            foreach(var parent in pass.Parents) {
                parent.Passcards.Remove(pass);
            }
            pass.Parents = null;
            await base.DeleteAsync(id);
        }
    }
}
