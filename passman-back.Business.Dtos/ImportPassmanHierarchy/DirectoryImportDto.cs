using passman_back.Domain.Core.DbEntities;

namespace passman_back.Business.Dtos.ImportPassmanHierarchy {
    public class DirectoryImportDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }

        public Directory Directory { get; private set; }

        public long SetDirectory(Directory directory) {
            this.Directory = directory;
            return directory.Id;
        }
    }
}