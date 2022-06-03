using System;

namespace passman_back.Domain.Core.DbEntities {
    public class RestorePasswordCode {
        public int Id { get; set; }
        public string RestoreCode { get; set; }
        public DateTime AliveBefore { get; set; }
        public virtual User User { get; set; }
        public long UserId { get; set; }
    }
}
