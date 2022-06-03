using System;

namespace passman_back.Business.Dtos {
    public class InviteCodeUpdateDto : AbstractDto {
        public string Name { get; set; }
        public bool IsStopped { get; set; }
        public DateTime AliveBefore { get; set; }
    }
}
