namespace passman_back.Business.Dtos {
    public class UserShortOutDto {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PatronymicName { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
