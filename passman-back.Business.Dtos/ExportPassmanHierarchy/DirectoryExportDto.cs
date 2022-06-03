namespace passman_back.Business.Dtos.ExportPassmanHierarchy {
    public class DirectoryExportDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
    }
}