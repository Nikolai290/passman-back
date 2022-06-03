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
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class PasscardService
        : BaseCrudService<Passcard, PasscardOutDto, PasscardCreateDto, PasscardUpdateDto>, IPasscardService {

        private readonly IPasscardRepository passcardRepository;
        private readonly IDirectoryRepository directoryRepository;
        private readonly ICrypter crypter;
        private readonly ISearcher searcher;

        public PasscardService(
            IPasscardRepository passcardRepository,
            IMapper mapper,
            IValidator<PasscardCreateDto> createValidator,
            IValidator<PasscardUpdateDto> updateValidator,
            IValidator<User> userValidator,
            IDirectoryRepository directoryRepository,
            ICrypter crypter,
            IHttpContextAccessor accessor,
            IUserRepository userRepository,
            ISearcher searcher
        ) : base(passcardRepository, mapper, createValidator, updateValidator, userValidator, userRepository, accessor) {
            this.passcardRepository = passcardRepository;
            this.directoryRepository = directoryRepository;
            this.crypter = crypter;
            this.searcher = searcher;
        }

        public override async Task<PasscardOutDto> CreateAsync(PasscardCreateDto createDto) {
            var user = await GetCurrentUserAndValidateAsync();
            await createValidator.ValidateAndThrowAsync(createDto);
            if (user.IsBlocked) {
                throw new UnauthorizedAccessException("Пользователь заблокирован");
            }
            if (!createDto.ParentIds.All(x => user.HasFullAccess(x))) {
                throw new UnauthorizedAccessException("Необходимо иметь полный доступ ко всем указанным папкам-родителям");
            }
            var passcard = mapper.Map<Passcard>(createDto);
            passcard.Parents = await directoryRepository.GetByIdsAsync(createDto.ParentIds);
            var result = await passcardRepository.CreateAsync(passcard);
            var outDto = mapper.Map<PasscardOutDto>(result);
            return outDto;
        }

        public override async Task<PasscardOutDto> UpdateAsync(PasscardUpdateDto updateDto) {
            var user = await GetCurrentUserAndValidateAsync();
            await updateValidator.ValidateAndThrowAsync(updateDto);
            if (user.IsAdmin) {
                return await UpdatePasscardByAdminAsync(updateDto);
            }

            if (!user.HasFullAccessToPasscard(updateDto.Id)) {
                throw new UnauthorizedAccessException();
            }
            var passcard = user.GetPasscardsBy().First(x => x.Id == updateDto.Id);
            var parents = await directoryRepository.GetByIdsAsync(updateDto.ParentIds);

            var checkNewIds = parents.Except(passcard.Parents).Select(x => x.Id).ToList();
            var checkRemoveIds = passcard.Parents.Except(parents).Select(x => x.Id);
            var mutableParentIds = new List<long>();
            mutableParentIds.AddRange(checkNewIds);
            mutableParentIds.AddRange(checkRemoveIds);

            if (mutableParentIds.Any() && !mutableParentIds.All(id => user.HasFullAccess(id))
            ) {
                throw new UnauthorizedAccessException("Изменения касаются родительских папок, к которым у вас нет полного доступа");
            }

            mapper.Map(updateDto, passcard);
            passcard.Parents = passcard.Parents.Restruct(parents);

            await passcardRepository.UpdateAsync(passcard);
            var outDto = mapper.Map<PasscardOutDto>(passcard);
            return outDto;
        }

        public override async Task DeleteAsync(long id) {
            var user = await GetCurrentUserAndValidateAsync();
            if (!user.HasFullAccessToPasscard(id)) {
                throw new UnauthorizedAccessException();
            }
            var passcard = await passcardRepository.GetByIdAsync(id);
            if (!passcard.Parents.Select(x => x.Id).All(x => user.HasFullAccess(x))) {
                throw new UnauthorizedAccessException("Необходимо иметь полный доступ ко всем папкам, где доступна эта запись");
            }
            passcard.Parents.Clear();
            await passcardRepository.DeleteAsync(id);
        }

        public async Task<IList<PasscardOutDto>> GetAllAsync(PasscardRequestDto requestDto) {
            var user = await GetCurrentUserAndValidateAsync();
            IList<PasscardOutDto> result = default;
            if (user.IsAdmin) {
                result = await GetTotallyAllPasscards(requestDto);
            } else {
                if (requestDto.DirectoryId > 0 && !user.HasAnyAccess(requestDto.DirectoryId)) {
                    throw new UnauthorizedAccessException("Нет доступа к указанной папке");
                }

                var passcards = user.GetPasscardsBy(requestDto.DirectoryId);
                var searchResult = passcards.SearchAnd(requestDto.Search, mapper, searcher);
                result = mapper.Map<IList<PasscardOutDto>>(searchResult);
            }

            foreach (var passcarOutDto in result) {
                passcarOutDto.IsFavorite
                    = user.FavoritePasscards.Any(x => x.Id == passcarOutDto.Id);
            }

            return result;
        }

        private async Task<PasscardOutDto> UpdatePasscardByAdminAsync(PasscardUpdateDto updateDto) {
            var passcard = await passcardRepository.GetByIdAsync(updateDto.Id);
            var parents = await directoryRepository.GetByIdsAsync(updateDto.ParentIds);
            mapper.Map(updateDto, passcard);
            passcard.Parents = passcard.Parents.Restruct(parents);
            await passcardRepository.UpdateAsync(passcard);
            var outDto = mapper.Map<PasscardOutDto>(passcard);
            return outDto;
        }

        private async Task<IList<PasscardOutDto>> GetTotallyAllPasscards(PasscardRequestDto requestDto) {
            var passcards = await passcardRepository.GetAllAsync();
            if (requestDto.DirectoryId > 0) {
                passcards = passcards.Where(x => x.Parents.Select(dir => dir.Id).Contains(requestDto.DirectoryId)).ToList();
            }
            passcards = passcards.SearchAnd(requestDto.Search, mapper, searcher).ToList();
            var result = mapper.Map<IList<PasscardOutDto>>(passcards);
            return result;
        }

        private string TryDecrypt(string password) {
            try {
                var decryptedPassword = crypter.Decrypt(password);
                return decryptedPassword;
            } catch {
                return password;
            }
        }
    }
}
