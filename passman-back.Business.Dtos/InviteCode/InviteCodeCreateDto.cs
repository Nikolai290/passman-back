using System;
using System.Collections.Generic;

namespace passman_back.Business.Dtos {
    public class InviteCodeCreateDto {
        public string Name { get; set; }
        public bool IsStopped { get; set; }
        public DateTime AliveBefore { get; set; }
        public string Role { get; set; }
        public IList<long> UserGroupIds { get; set; }
    }
}
