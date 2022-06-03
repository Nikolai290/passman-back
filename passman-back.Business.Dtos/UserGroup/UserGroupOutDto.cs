namespace passman_back.Business.Dtos {
    public class UserGroupOutDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public long[] UserIds { get; set; }
        public UserGroupDirectoryRelationOutDto[] Relations { get; set; }
    }
}
