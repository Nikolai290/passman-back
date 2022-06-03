using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IImportService {
        Task ImportHierarchyPassmanAsync(IFormFile file);
        Task ImportHierarchyBitwardenAsync(IFormFile file);
        Task ImportAsync(IFormFile file);
    }
}
