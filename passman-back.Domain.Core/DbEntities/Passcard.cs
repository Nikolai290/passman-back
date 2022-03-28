using System.Collections.Generic;

namespace passman_back.Domain.Core.DbEntities {
    public class Passcard : AbstractDbEntity {
        public IList<Directory> Parents { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
    }
}