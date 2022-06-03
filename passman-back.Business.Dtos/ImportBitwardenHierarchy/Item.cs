namespace passman_back.Business.Dtos.ImportBitwardenHierarchy {
    public class Item {
        public string Id { get; set; }
        public string FolderId { get; set; }
        public string Name { get; set; }
        public Login Login { get; set; }
    }
}