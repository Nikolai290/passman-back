using passman_back.Domain.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace passman_back.Domain.Core.DbEntities {
    public class User : AbstractDbEntity {
        public virtual string Nickname { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PatronymicName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsBlocked { get; set; }

        public virtual Role Role { get; set; }
        public virtual RestorePasswordCode RestorePasswordCode { get; set; }
        public virtual IList<UserGroup> UserGroups { get; set; }
        public virtual IList<Passcard> FavoritePasscards { get; set; }

        public bool IsAdmin => Role == Role.Admin;

        public HashSet<UserGroupDirectoryRelation> GetHashSetRelations() {
            var result = new HashSet<UserGroupDirectoryRelation>();
            var relationsGroupedByDirectory = this.UserGroups
                .Where(x => !x.IsDeleted)
                .SelectMany(x => x.Relations)
                .Where(x => x.Permission != Permission.None && !x.IsDeleted && !x.Directory.IsDeleted)
                .GroupBy(x => x.Directory.Id);

            foreach (var group in relationsGroupedByDirectory) {
                var maxPerm = group.Max(x => x.Permission);
                result.Add(group.First(x => x.Permission == maxPerm));
            }

            return result;
        }

        public Permission GetPermissionForDirectory(long directoryId) {
            var relationsForDirectory = UserGroups
                .Where(x => !x.IsDeleted)
                .SelectMany(x => x.Relations)
                .Where(x => x.Directory.Id == directoryId && !x.IsDeleted && !x.Directory.IsDeleted);
            if (!relationsForDirectory.Any()) {
                return Permission.None;
            }
            return relationsForDirectory.Max(x => x.Permission);
        }

        public bool HasFullAccess(long directoryId) {
            return Role == Role.Admin
                || GetPermissionForDirectory(directoryId) == Permission.FullAccess;
        }

        public bool HasAnyAccess(long directoryId) {
            return Role == Role.Admin
                || GetPermissionForDirectory(directoryId) > Permission.None;
        }

        public bool HasFullAccessToPasscard(long passcardId) {
            return IsAdmin
                || GetHashSetRelations()
                .Where(x => x.Permission == Permission.FullAccess)
                .Select(x => x.Directory)
                .Where(x => !x.IsDeleted)
                .SelectMany(x => x.Passcards)
                .Any(x => x.Id == passcardId && !x.IsDeleted);
        }

        public bool HasAnyAccessToPasscard(long passcardId) {
            return IsAdmin || GetHashSetRelations()
                .Where(x => x.Permission > Permission.None)
                .Select(x => x.Directory)
                .Where(x => !x.IsDeleted)
                .Where(x => !x.HasDeletedParent())
                .SelectMany(x => x.Passcards)
                .Any(x => x.Id == passcardId && !x.IsDeleted);
        }


        public IEnumerable<Passcard> GetPasscardsBy(long directoryId = 0) {
            var result = UserGroups
                .Where(x => !x.IsDeleted)
                .SelectMany(x => x.Relations)
                .Where(x => !x.IsDeleted && x.Permission > Permission.None)
                .Select(x => x.Directory)
                .Where(x => !x.IsDeleted)
                .Where(x => directoryId > 0 ? x.Id == directoryId : true)
                .SelectMany(x => x.Passcards)
                .Where(x => !x.IsDeleted)
                .DistinctBy(x => x.Id);
            return result;
        }
    }
}
