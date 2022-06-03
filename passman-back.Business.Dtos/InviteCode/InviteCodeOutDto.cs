using passman_back.Domain.Core.Enums;
using System;
using System.Collections.Generic;

namespace passman_back.Business.Dtos {
    public class InviteCodeOutDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsStopped { get; set; }
        public bool isActive { get; set; }
        public DateTime AliveBefore { get; set; }
        public string Role { get; set; }
        public IList<UserGroupShortOutDto> UserGroups { get; set; }
    }
}
