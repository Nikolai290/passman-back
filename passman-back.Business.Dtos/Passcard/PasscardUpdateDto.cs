namespace passman_back.Business.Dtos {
    public class PasscardUpdateDto {
        public long[] ParentIds { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
    }
}