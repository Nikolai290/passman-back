using passman_back.Domain.Core.Enums;

namespace passman_back.Domain.Core.DbEntities {
    public class UserGroupDirectoryRelation : AbstractDbEntity {
        public virtual UserGroup UserGroup { get; set; }
        public virtual Directory Directory { get; set; }
        public virtual Permission Permission { get; set; }

    }
}