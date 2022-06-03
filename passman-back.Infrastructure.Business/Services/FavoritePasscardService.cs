using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class FavoritePasscardService : BaseService, IFavoritePasscardService {
        private readonly IPasscardRepository passcardRepository;
        private readonly IMapper mapper;
        public FavoritePasscardService(
            IPasscardRepository passcardRepository,
            IUserRepository userRepository,
            IHttpContextAccessor accessor,
            IValidator<User> userValidator,
            IMapper mapper
        ) : base(userRepository, accessor, userValidator) {
            this.passcardRepository = passcardRepository;
            this.mapper = mapper;
        }

        public async Task<PasscardOutDto[]> GetAllAsync() {
            var user = await GetCurrentUserAndValidateAsync();
            var favoritePasscards = user.FavoritePasscards
                .Where(x => !x.IsDeleted)
                .Where(x => x.Parents.Any(y => !y.HasDeletedParent()))
                .Where(x => user.HasAnyAccessToPasscard(x.Id));

            var outDtos = mapper.Map<List<PasscardOutDto>>(favoritePasscards);
            outDtos.ForEach(x => x.IsFavorite = true);
            return outDtos.ToArray();
        }

        public async Task AddAsync(long id) {
            var user = await GetCurrentUserAndValidateAsync();

            if (user.FavoritePasscards.Any(x => x.Id == id)) {
                return;
            }
            if (!user.HasAnyAccessToPasscard(id)) {
                throw new UnauthorizedAccessException("Нет доступа к этой пасскарде");
            }

            var passcard = await passcardRepository.GetByIdAsync(id);
            user.FavoritePasscards.Add(passcard);
            await userRepository.UpdateAsync(user);
        }



        public async Task RemoveAsync(long id) {
            var user = await GetCurrentUserAndValidateAsync();

            if (!user.FavoritePasscards.Any(x => x.Id == id)) {
                return;
            }
            if (!user.HasAnyAccessToPasscard(id)) {
                throw new UnauthorizedAccessException("Нет доступа к этой пасскарде");
            }

            var passcard = await passcardRepository.GetByIdAsync(id);
            user.FavoritePasscards.Remove(passcard);
            await userRepository.UpdateAsync(user);
        }
    }
}