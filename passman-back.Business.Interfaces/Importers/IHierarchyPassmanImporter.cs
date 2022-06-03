using passman_back.Business.Dtos.ImportPassmanHierarchy;
using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;

namespace passman_back.Business.Interfaces.Importers {
    public interface IHierarchyPassmanImporter {
        IList<Passcard> GetPascards(
            PassmanHierarchyImportDto importDto,
            IList<Domain.Core.DbEntities.Directory> directories
        );
        IList<Domain.Core.DbEntities.Directory> GetDirectories(
            IList<DirectoryImportDto> importDirectoryDtos
        );
    }
}