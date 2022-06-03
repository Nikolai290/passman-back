using passman_back.Domain.Core.Enums;

namespace passman_back.Business.Dtos {
    public class UserAdminUpdateDto : AbstractDto {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
        public long[] UserGroupIds { get; set; }
    }
}
