using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;
using System.IO;

namespace passman_back.Business.Interfaces.Exporters {
    public interface IHierarchyPassmanExporter {
        void ExportHierarchyPassmanJson(MemoryStream stream, bool encrypted, IList<Domain.Core.DbEntities.Directory> directories, IList<Passcard> passcards);
    }
}