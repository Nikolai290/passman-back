namespace passman_back.Business.Dtos {
    public class UserGroupUpdateDto : AbstractDto {
        public string Name { get; set; }
        public long[] UserIds { get; set; }
    }
}
