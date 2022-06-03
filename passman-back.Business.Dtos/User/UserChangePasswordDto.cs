namespace passman_back.Business.Dtos {
    public class UserChangePasswordDto {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
