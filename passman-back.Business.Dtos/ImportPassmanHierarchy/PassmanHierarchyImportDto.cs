using System.Collections.Generic;
namespace passman_back.Business.Dtos.ImportPassmanHierarchy {
    public class PassmanHierarchyImportDto {
        public bool Encrypted { get; set; }
        public IList<DirectoryImportDto> Directories { get; set; }
        public IList<PasscardImportDto> Passcards { get; set; }

        public PassmanHierarchyImportDto() {
            Directories = new List<DirectoryImportDto>();
            Passcards = new List<PasscardImportDto>();
        }
    }
}