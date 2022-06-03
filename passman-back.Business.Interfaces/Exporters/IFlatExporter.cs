using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;
using System.IO;

namespace passman_back.Business.Interfaces.Exporters {
    public interface IFlatExporter {
        void ExportToXlsx(IList<Passcard> passcards, MemoryStream stream, string[] headers);
        void ExportToCsv(IEnumerable<Passcard> passcards, Stream stream, string[] headers);
    }
}