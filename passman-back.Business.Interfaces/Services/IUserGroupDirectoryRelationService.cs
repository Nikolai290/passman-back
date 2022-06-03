using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;

namespace passman_back.Business.Interfaces.Services {
    public interface IUserGroupDirectoryRelationService
        : IBaseCrudService<
            UserGroupDirectoryRelation,
            UserGroupDirectoryRelationOutDto,
            UserGroupDirectoryRelationCreateDto,
            UserGroupDirectoryRelationUpdateDto
            > { }
}
