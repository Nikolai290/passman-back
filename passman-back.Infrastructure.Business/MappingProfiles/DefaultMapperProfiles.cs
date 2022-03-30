using AutoMapper;
using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;

namespace passman_back.Infrastructure.Business.MappingProfiles {
    public class DefaultMapperProfiles : Profile {
        public DefaultMapperProfiles() {
            // Directories
            CreateMap<Directory, DirectoryOutDto>();
            CreateMap<Directory, DirectoryShortOutDto>();
            CreateMap<DirectoryCreateDto, Directory>();
            CreateMap<DirectoryUpdateDto, Directory>();

            // Passcards
            CreateMap<Passcard, PasscardOutDto>();
            CreateMap<PasscardCreateDto, Passcard>();
            CreateMap<PasscardUpdateDto, Passcard>();
        }
    }
}
