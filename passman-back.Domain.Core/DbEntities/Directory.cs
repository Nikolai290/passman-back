using System.Collections.Generic;

namespace passman_back.Domain.Core.DbEntities {
    public class Directory : AbstractDbEntity {
        public Directory() { }
        public virtual string Name { get; set; }
        public virtual Directory Parent { get; set; }
        public virtual IList<Directory> Childrens { get; set; }
        public virtual IList<Passcard> Passcards { get; set; }
    }
}
