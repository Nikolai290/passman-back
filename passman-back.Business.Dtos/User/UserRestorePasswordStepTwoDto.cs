namespace passman_back.Business.Dtos {
    public class UserRestorePasswordStepTwoDto {
        public string RestorePasswordCode { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
