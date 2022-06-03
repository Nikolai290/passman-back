using passman_back.Domain.Core.Enums;

namespace passman_back.Business.Dtos {
    public class UserGroupDirectoryRelationCreateDto {
        public long DirectoryId { get; set; }
        public Permission Permission { get; set; }
    }
}