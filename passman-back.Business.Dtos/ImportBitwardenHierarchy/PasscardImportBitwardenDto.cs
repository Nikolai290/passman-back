namespace passman_back.Business.Dtos.ImportBitwardenHierarchy {
    public class PasscardImportBitwardenDto {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string FolderId { get; set; }
    }
}