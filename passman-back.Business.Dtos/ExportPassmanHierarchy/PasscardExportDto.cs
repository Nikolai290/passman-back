namespace passman_back.Business.Dtos.ExportPassmanHierarchy {
    public class PasscardExportDto {
        public long Id { get; set; }
        public long[] ParentIds { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
    }
    public class PasscardExportCryptedDto : PasscardExportDto { }
}