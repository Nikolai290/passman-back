namespace passman_back.Business.Dtos {
    public class UserGroupMatrixOutDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public UserGroupDirectoryRelationOutDto[] Relations { get; set; }
    }
}
