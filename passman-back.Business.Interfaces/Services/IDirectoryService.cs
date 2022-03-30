using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IDirectoryService
        : IBaseCrudService<Directory, DirectoryOutDto, DirectoryCreateDto, DirectoryUpdateDto> {
        Task<IList<DirectoryShortOutDto>> GetAllShortAsync();
    }
}
