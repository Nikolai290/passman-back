using System.Collections.Generic;

namespace passman_back.Domain.Core.DbEntities {
    public class Passcard : AbstractDbEntity {
        public Passcard() { }
        public virtual IList<Directory> Parents { get; set; }
        public virtual string Url { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Description { get; set; }
    }
}