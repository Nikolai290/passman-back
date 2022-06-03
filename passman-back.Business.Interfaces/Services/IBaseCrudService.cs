using passman_back.Business.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IBaseCrudService<TEntity, TOutDto, TCreateDto, TUpdateDto> {

        Task<IList<TOutDto>> GetAllAsync();
        Task<TOutDto> GetByIdAsync(long id);
        Task<TOutDto> CreateAsync(TCreateDto createDto);
        Task<TOutDto> UpdateAsync(TUpdateDto updateDto);

        /// <summary>
        /// Pseudo-Delete
        /// </summary>
        Task DeleteAsync(long id);
    }
}
