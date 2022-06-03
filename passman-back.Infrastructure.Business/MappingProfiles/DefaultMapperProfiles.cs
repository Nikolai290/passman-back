using AutoMapper;
using passman_back.Business.Dtos;
using passman_back.Business.Dtos.ExportPassmanHierarchy;
using passman_back.Business.Dtos.ImportBitwardenHierarchy;
using passman_back.Business.Dtos.ImportPassmanHierarchy;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using passman_back.Infrastructure.Business.Cryptography;
using SearchingLibrary.Models;
using System.Linq;

namespace passman_back.Infrastructure.Business.MappingProfiles {
    public class DefaultMapperProfiles : Profile {

        public DefaultMapperProfiles() {
            // Directories
            CreateMap<Directory, DirectoryOutDto>()
                .ForMember(x => x.ParentId,
                    opt => opt.MapFrom(x => x.Parent.Id));
            CreateMap<Directory, DirectoryShortOutDto>();
            CreateMap<Directory, DirectoryExportDto>()
                .ForMember(
                    x => x.ParentId,
                    opt => opt.MapFrom(x => x.Parent.Id));
            CreateMap<DirectoryCreateDto, Directory>();
            CreateMap<DirectoryUpdateDto, Directory>();
            CreateMap<DirectoryImportDto, Directory>()
                .ForMember(
                    x => x.Id,
                    opt => opt.MapFrom((src, dest, value) => src.SetDirectory(dest)));
            CreateMap<Folder, Directory>()
                .ForMember(
                    x => x.Id,
                    opt => opt.MapFrom(x => 0)
                );

            // Passcards
            CreateMap<Passcard, PasscardOutDto>()
                .ForMember(
                    x => x.ParentIds,
                    opt => opt.MapFrom(x => x.Parents.Select(parent => parent.Id)))
                .ForMember(
                    x => x.Password,
                    opt => opt.MapFrom(x => TryDecrypt(x.Password, new Crypter())));
            CreateMap<Passcard, PasscardExportDto>()
                    .ForMember(
                        x => x.ParentIds,
                        opt => opt.MapFrom(x => x.Parents.Select(parent => parent.Id)))
                    .ForMember(
                        x => x.Password,
                        opt => opt.MapFrom(x => TryDecrypt(x.Password, new Crypter())));
            CreateMap<Passcard, PasscardExportCryptedDto>()
                    .ForMember(
                        x => x.ParentIds,
                        opt => opt.MapFrom(x => x.Parents.Select(parent => parent.Id)))
                    .ForMember(
                        x => x.Password,
                        opt => opt.MapFrom(x => x.Password));
            CreateMap<PasscardExportDto, PasscardExportCryptedDto>();
            CreateMap<Passcard, SearchableEntity>()
                .ForMember(
                    x => x.SearchableProperty,
                    opt => opt.MapFrom(x => string.Join(' ', x.Name, x.Url, x.Login, x.Description).ToLower())
                );

            CreateMap<PasscardCreateDto, Passcard>()
                .ForMember(
                    x => x.Password,
                    opt => opt.MapFrom(x => TryEncrypt(x.Password, new Crypter())));
            CreateMap<PasscardImportDto, Passcard>()
                .ForMember(
                    x => x.Password,
                    opt => opt.MapFrom(x => TryDecryptThenEncrypt(x.Password, new Crypter())
                ))
                .ForMember(
                    x => x.Id,
                    opt => opt.MapFrom(x => 0)
                );
            CreateMap<Item, PasscardImportBitwardenDto>()
                .ForMember(
                    x => x.Login,
                    opt => opt.MapFrom(x => x.Login.UserName)
                )
                .ForMember(
                    x => x.Password,
                    opt => opt.MapFrom(x => x.Login.Password)
                )
                .ForMember(
                    x => x.Url,
                    opt => opt.MapFrom(x => "")
                );
            CreateMap<PasscardImportBitwardenDto, Passcard>();

            CreateMap<PasscardUpdateDto, Passcard>()
                .ForMember(
                    x => x.Password,
                    opt => opt.MapFrom((src, dest, value) =>
                        string.IsNullOrEmpty(src.Password)
                        ? dest?.Password
                        : TryEncrypt(src.Password, new Crypter())));

            // Users
            CreateMap<User, UserOutDto>()
                .ForMember(
                    x => x.Role,
                    opt => opt.MapFrom((src, dest, value) =>
                        src.Role == Role.Admin
                        ? Roles.Admin
                        : Roles.User));
            CreateMap<User, UserShortOutDto>()
                .ForMember(
                    x => x.Role,
                    opt => opt.MapFrom((src, dest, value) =>
                        src.Role == Role.Admin
                        ? Roles.Admin
                        : Roles.User));
            CreateMap<UserRegisterDto, User>()
                .ForMember(
                    x => x.Password,
                    opt => opt.MapFrom(x => TryEncrypt(x.Password, new Crypter())))
                .ForMember(
                    x => x.Email,
                    opt => opt.MapFrom(x => x.Email.ToLower()));
            CreateMap<UserUpdateDto, User>()
                .ForMember(
                    x => x.Email,
                    opt => opt.MapFrom(x => x.Email.ToLower()));
            CreateMap<UserAdminUpdateDto, User>()
                .ForMember(
                    x => x.Role,
                    opt => opt.MapFrom((src, dest, value) => src.Role.ToLower().Equals("admin") ? Role.Admin : Role.User));
            CreateMap<UserAdminCreateDto, User>()
                .ForMember(
                    x => x.Email,
                    opt => opt.MapFrom(x => x.Email.ToLower()))
                .ForMember(
                    x => x.Role,
                    opt => opt.MapFrom((src, dest, value) => src.Role.ToLower().Equals("admin") ? Role.Admin : Role.User));

            // UserGroups
            CreateMap<UserGroup, UserGroupOutDto>()
                .ForMember(x => x.UserIds,
                    opt => opt.MapFrom(x => x.Users.Select(x => x.Id)))
                .ForMember(x => x.Relations,
                    opt => opt.MapFrom((src, dest, value) => src.Relations.Where(x => !x.IsDeleted)));
            CreateMap<UserGroup, UserGroupShortOutDto>();
            CreateMap<UserGroup, UserGroupMatrixOutDto>();
            CreateMap<UserGroupCreateDto, UserGroup>();
            CreateMap<UserGroupUpdateDto, UserGroup>();

            // UserGroupsDirectoryRelations
            CreateMap<UserGroupDirectoryRelation, UserGroupDirectoryRelationOutDto>()
                .ForMember(
                    x => x.DirectoryId,
                    opt => opt.MapFrom((src, dest, value) => src.Directory.Id))
                .ForMember(
                    x => x.Permission,
                    opt => opt.MapFrom((src, dest, value) => src.Permission));
            CreateMap<UserGroupDirectoryRelationCreateDto, UserGroupDirectoryRelation>();
            CreateMap<UserGroupDirectoryRelationUpdateDto, UserGroupDirectoryRelation>();

            // Invite codes
            CreateMap<InviteCode, InviteCodeOutDto>()
                .ForMember(
                    x => x.Role,
                    opt => opt.MapFrom(x => x.Role == Role.Admin ? "admin" : "user"));
            CreateMap<InviteCodeCreateDto, InviteCode>()
                .ForMember(
                    x => x.Role,
                    opt => opt.MapFrom(x => x.Role.Equals("admin") ? Role.Admin : Role.User));
            CreateMap<InviteCodeUpdateDto, InviteCode>();



            // Searchable entity
            CreateMap<User, SearchableEntity<long>>()
                .ForMember(x => x.SearchableProperty,
                    opt => opt.MapFrom((src, dest, value) => string.Join(' ',
                        src?.Nickname,
                        src?.Email,
                        src?.FirstName,
                        src?.LastName,
                        src?.PatronymicName,
                        src?.Role
                    )));

            CreateMap<UserGroup, SearchableEntity<long>>()
                .ForMember(x => x.SearchableProperty,
                    opt => opt.MapFrom((src, dest, value) => string.Join(' ',
                        src.Name
                    )));

            CreateMap<Directory, SearchableEntity<long>>()
                .ForMember(x => x.SearchableProperty,
                    opt => opt.MapFrom((src, dest, value) => string.Join(' ',
                        src.Name
                    )));
            CreateMap<Passcard, SearchableEntity<long>>()
                .ForMember(x => x.SearchableProperty,
                    opt => opt.MapFrom((src, dest, value) => string.Join(' ',
                        src.Name,
                        src.Description,
                        src.Url,
                        src.Login
                    )));
        }


        private string TryDecrypt(string p, ICrypter crypter) {
            try {
                return crypter.Decrypt(p);
            } catch {
                return p;
            }
        }

        private string TryDecryptThenEncrypt(string p, ICrypter crypter) {
            var foo = TryDecrypt(p, crypter);
            return TryEncrypt(foo, crypter);
        }


        private string TryEncrypt(string p, ICrypter crypter) {
            if (string.IsNullOrEmpty(p)) {
                return null;
            }
            try {
                return crypter.Encrypt(p);
            } catch {
                return p;
            }
        }
    }
}
