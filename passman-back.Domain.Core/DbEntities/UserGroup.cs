using System.Collections.Generic;
using System.Linq;

namespace passman_back.Domain.Core.DbEntities {
    public class UserGroup : AbstractDbEntity {
        public virtual string Name { get; set; }
        public virtual IList<User> Users { get; set; }
        public virtual IList<UserGroupDirectoryRelation> Relations { get; set; }
        public virtual IList<InviteCode> InviteCodes { get; set; }

        public void ExcludeUsersWhoNotContainsInList(IEnumerable<long> userIds) {
            foreach (var user in Users) {
                if (!userIds.Contains(user.Id)) {
                    user.UserGroups.Remove(this);
                }
            }
        }

        public void AddUsers(IEnumerable<User> users) {
            foreach (var user in users) {
                if (this.Users.Contains(user)) continue;
                this.Users.Add(user);
                user.UserGroups.Add(this);
            }
        }

        public void UpdateUserList(IList<User> users) {
            ExcludeUsersWhoNotContainsInList(users.Select(x => x.Id));
            AddUsers(users);
        }
    }
}