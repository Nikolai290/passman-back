using passman_back.Domain.Core.Enums;

namespace passman_back.Business.Dtos {
    public class UserGroupDirectoryRelationUpdateDto : AbstractDto {
        private new long Id { get; set; }
        public long DirectoryId { get; set; }
        public Permission Permission { get; set; }
    }
}