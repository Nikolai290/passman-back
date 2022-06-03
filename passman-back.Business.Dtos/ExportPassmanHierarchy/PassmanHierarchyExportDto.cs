using System.Collections.Generic;
namespace passman_back.Business.Dtos.ExportPassmanHierarchy {
    public class PassmanHierarchyExportDto {
        public bool Encrypted { get; set; }
        public IList<DirectoryExportDto> Directories { get; set; }
        public IList<PasscardExportDto> Passcards { get; set; }

        public PassmanHierarchyExportDto() {
            Directories = new List<DirectoryExportDto>();
            Passcards = new List<PasscardExportDto>();
        }
    }
}