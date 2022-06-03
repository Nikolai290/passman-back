namespace passman_back.Business.Dtos {
    public class UserGroupCreateDto {
        public string Name { get; set; }
        public long[] UserIds { get; set; }
        public UserGroupDirectoryRelationCreateDto[] Relations { get; set; }
    }
}
