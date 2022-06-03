using passman_back.Domain.Core.Enums;
using System;
using System.Collections.Generic;

namespace passman_back.Domain.Core.DbEntities {
    public class InviteCode : AbstractDbEntity {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsStopped { get; set; }
        public DateTime AliveBefore { get; set; }
        public Role Role { get; set; }
        public virtual IList<UserGroup> UserGroups { get; set; }

        public bool isActive => !IsStopped && DateTime.Now < AliveBefore && !IsDeleted;
    }
}
