namespace passman_back.Business.Dtos {
    public class UserUpdateDto : AbstractDto {
        public long Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public void SetId(long id) => Id = id;
    }
}
