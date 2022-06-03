using System.IO;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IExportService {
        Task ExportAsync(string type, MemoryStream stream);
        Task ExportHierarchyPassmanAsync(MemoryStream stream, bool encrypted);
    }
}
