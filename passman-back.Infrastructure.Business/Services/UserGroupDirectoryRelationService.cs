using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;

namespace passman_back.Infrastructure.Business.Services {
    public class UserGroupDirectoryRelationService
        : BaseCrudService<
            UserGroupDirectoryRelation,
            UserGroupDirectoryRelationOutDto,
            UserGroupDirectoryRelationCreateDto,
            UserGroupDirectoryRelationUpdateDto
            >,
        IUserGroupDirectoryRelationService {
        public UserGroupDirectoryRelationService(
            IUserGroupDirectoryRelationRepository baseCrudRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<UserGroupDirectoryRelationCreateDto> createValidator,
            IValidator<UserGroupDirectoryRelationUpdateDto> updateValidator,
            IValidator<User> userValidator,
            IHttpContextAccessor accessor

        ) : base(baseCrudRepository, mapper, createValidator, updateValidator, userValidator, userRepository, accessor) {
        }
    }
}
