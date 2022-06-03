using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;

namespace passman_back.Domain.Interfaces.DbContexts {
    public interface IMainDbContext {
        DbSet<Directory> Directories { get; set; }
        DbSet<Passcard> Passcards { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserGroup> UserGroups { get; set; }
        DbSet<UserGroupDirectoryRelation> UserGroupDirectoryRelations { get; set; }
        DbSet<RestorePasswordCode> RestorePasswordCodes { get; set; }
        DbSet<InviteCode> InviteCodes { get; set; }
    }
}
