using System.Collections.Generic;

namespace passman_back.Domain.Core.DbEntities {
    public class Directory : AbstractDbEntity {
        public string Name { get; set; }
        public Directory Parent { get; set; }
        public IList<Directory> Childrens { get; set; }
        public IList<Passcard> Passcards { get; set; }
    }
}
