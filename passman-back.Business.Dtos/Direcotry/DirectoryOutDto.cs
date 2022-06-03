using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace passman_back.Business.Dtos {
    public class DirectoryOutDto {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Name { get; set; }
        public Permission Permission { get; set; }
        public IList<DirectoryOutDto> Childrens { get; set; }

        public void ExcludeChildrenWhoHasNotInList(IEnumerable<long> ids) {
            if (Childrens is null || Childrens.Count == 0) return;
            Childrens = Childrens.Where(child => ids.Contains(child.Id)).ToList();
            foreach (var child in Childrens) {
                child.ExcludeChildrenWhoHasNotInList(ids);
            }
        }

        public bool ContainsChildrenWithId(long id) {
            if (Childrens is null || Childrens.Count == 0) return false;
            if (Childrens.Any(x => x.Id == id)) return true;
            return Childrens.Any(x => x.ContainsChildrenWithId(id));
        }

        public void SetPermissionForAllChildren(IEnumerable<UserGroupDirectoryRelation> relations) {
            var relation = relations.First(x => x.Directory.Id == Id);
            Permission = relation.Permission;
            Childrens.ToList().ForEach(child => child.SetPermissionForAllChildren(relations));
        }

        public void SetPermissionForAllChildren(Permission permission) {
            Permission = permission;
            foreach(var child in Childrens) {
                child.SetPermissionForAllChildren(permission);
            }
        }
    }
}
