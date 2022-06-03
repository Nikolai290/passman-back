using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class BaseCrudService<TEntity, TOutDto, TCreateDto, TUpdateDto>
        : BaseService, IBaseCrudService<TEntity, TOutDto, TCreateDto, TUpdateDto>
        where TEntity : AbstractDbEntity
        where TUpdateDto : AbstractDto {

        protected readonly IBaseCrudRepository<TEntity> baseCrudRepository;
        protected readonly IMapper mapper;
        protected readonly IValidator<TCreateDto> createValidator;
        protected readonly IValidator<TUpdateDto> updateValidator;

        public BaseCrudService(
            IBaseCrudRepository<TEntity> baseCrudRepository,
            IMapper mapper,
            IValidator<TCreateDto> createValidator,
            IValidator<TUpdateDto> updateValidator,
            IValidator<User> userValidator,
            IUserRepository userRepository,
            IHttpContextAccessor accessor
        ) : base(userRepository, accessor, userValidator) {
            this.baseCrudRepository = baseCrudRepository;
            this.mapper = mapper;
            this.createValidator = createValidator;
            this.updateValidator = updateValidator;
        }

        public virtual async Task<IList<TOutDto>> GetAllAsync() {
            await CheckUserIsBlocked();
            var entities = await baseCrudRepository.GetAllAsync();
            var outDtos = mapper.Map<IList<TOutDto>>(entities);
            return outDtos;
        }

        public virtual async Task<TOutDto> GetByIdAsync(long id) {
            await CheckUserIsBlocked();
            var entity = await baseCrudRepository.GetByIdAsync(id);
            var outDto = mapper.Map<TOutDto>(entity);
            return outDto;
        }

        public virtual async Task<TOutDto> CreateAsync(TCreateDto createDto) {
            await CheckUserIsBlocked();
            await createValidator.ValidateAndThrowAsync(createDto);
            var entity = mapper.Map<TEntity>(createDto);
            var result = await baseCrudRepository.CreateAsync(entity);
            var outDto = mapper.Map<TOutDto>(result);
            return outDto;
        }

        public virtual async Task<TOutDto> UpdateAsync(TUpdateDto updateDto) {
            await CheckUserIsBlocked();
            await updateValidator.ValidateAndThrowAsync(updateDto);
            var entity = await baseCrudRepository.GetByIdAsync(updateDto.Id);
            mapper.Map(updateDto, entity);
            await baseCrudRepository.UpdateAsync(entity);
            var outDto = mapper.Map<TOutDto>(entity);
            return outDto;
        }

        public virtual async Task DeleteAsync(long id) {
            await CheckUserIsBlocked();
            await baseCrudRepository.DeleteAsync(id);
        }
    }
}
