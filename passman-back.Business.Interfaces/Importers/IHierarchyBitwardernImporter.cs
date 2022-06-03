using passman_back.Business.Dtos.ImportBitwardenHierarchy;
using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;

namespace passman_back.Business.Interfaces.Importers {
    public interface IHierarchyBitwardernImporter {
        IList<Passcard> GetPascards(
            BitwardenHierarchyDto importDto,
            IList<Directory> directories
        );
        IList<Directory> GetDirectories(IList<Folder> folders);
    }
}