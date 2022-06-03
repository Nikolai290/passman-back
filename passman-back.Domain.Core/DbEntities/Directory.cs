using System.Collections.Generic;
using System.Linq;

namespace passman_back.Domain.Core.DbEntities {
    public class Directory : AbstractDbEntity {
        public virtual string Name { get; set; }
        public virtual Directory Parent { get; set; }
        public virtual IList<Directory> Childrens { get; set; }
        public virtual IList<Passcard> Passcards { get; set; }
        public virtual IList<UserGroupDirectoryRelation> Relations { get; set; }

        public bool HasChildrenWithName(string name) {
            return Childrens.Any(child => child.Name == name);
        }

        public bool IsMyParent(Directory directory) {
            if (Parent is null) return false;
            if (Parent.Id == directory.Id) return true;
            return Parent.IsMyParent(directory);
        }

        public bool HasDeletedParent() {
            if (Parent is null) return false;
            if (Parent.IsDeleted) return true;
            if (IsDeleted) return true;
            return Parent.HasDeletedParent();
        }

        public void SetDeleteAllChildrens(bool isDelete = true) {
            IsDeleted = isDelete;
            foreach (var child in Childrens) {
                child.SetDeleteAllChildrens(isDelete);
            }
            if (isDelete) {
                foreach (var passcard in Passcards) {
                    if (passcard.Parents.All(x => x.Id == Id)) {
                        passcard.IsDeleted = true;
                    }
                }
                Passcards.Clear();
                Childrens.Clear();
            }
        }
    }
}
