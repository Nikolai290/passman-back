using Microsoft.AspNetCore.Http;
using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Importers {
    public interface IFlatImporter {
        Task<IEnumerable<Passcard>> ImportFromCsv(IFormFile file, Domain.Core.DbEntities.Directory directory);
        Task<IEnumerable<Passcard>> ImportFromXlsx(IFormFile file, Domain.Core.DbEntities.Directory directory);
    }
}