using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class InviteCodeService
        : BaseCrudService<InviteCode, InviteCodeOutDto, InviteCodeCreateDto, InviteCodeUpdateDto>, IInviteCodeService {
        private readonly IInviteCodeRepository inviteCodeRepository;
        private readonly IUserGroupsRepository userGroupsRepository;
        public InviteCodeService(
            IInviteCodeRepository baseCrudRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<InviteCodeCreateDto> createValidator,
            IValidator<InviteCodeUpdateDto> updateValidator,
            IValidator<User> userValidator,
            IUserGroupsRepository userGroupsRepository,
            IHttpContextAccessor accessor
        ) : base(baseCrudRepository, mapper, createValidator, updateValidator, userValidator, userRepository, accessor) {
            this.inviteCodeRepository = baseCrudRepository;
            this.userGroupsRepository = userGroupsRepository;
        }


        public override async Task<InviteCodeOutDto> CreateAsync(InviteCodeCreateDto createDto) {
            await GetCurrentUserAndValidateAsync();
            await createValidator.ValidateAndThrowAsync(createDto);
            var exists = await inviteCodeRepository.GetAllAsync();
            var invite = mapper.Map<InviteCode>(createDto);
            invite.UserGroups = await userGroupsRepository.GetByIdsAsync(createDto.UserGroupIds);

            invite.Code = GenerateCode();
            while(exists.Any(x => x.Code.Equals(invite.Code))) {
                invite.Code = GenerateCode();
            }

            var result = await baseCrudRepository.CreateAsync(invite);
            var outDto = mapper.Map<InviteCodeOutDto>(result);
            return outDto;
        }

        public async Task DisableAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var invite = await inviteCodeRepository.GetByIdAsync(id);
            invite.IsStopped = true;
            await inviteCodeRepository.UpdateAsync(invite);
        }

        public async Task EnableAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var invite = await inviteCodeRepository.GetByIdAsync(id);
            invite.IsStopped = false;
            await inviteCodeRepository.UpdateAsync(invite);
        }

        public override async Task DeleteAsync(long id) {
            await GetCurrentUserAndValidateAsync();
            var invite = await inviteCodeRepository.GetByIdAsync(id);
            invite.UserGroups.Clear();
            await inviteCodeRepository.UpdateAsync(invite);
            await inviteCodeRepository.DeleteAsync(id);
        }

        private string GenerateCode() {
            var rnd = new Random();
            var num1 = rnd.Next(1000000, 9999999);
            var num2 = rnd.Next(1000000, 9999999);
            var num3 = num1 ^ num2;
            var hex = num3.ToString("X") + num1.ToString("X") + num2.ToString("X");
            return hex;
        }
    }
}
