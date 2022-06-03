using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IPasscardService
        : IBaseCrudService<Passcard, PasscardOutDto, PasscardCreateDto, PasscardUpdateDto> {
        Task<IList<PasscardOutDto>> GetAllAsync(PasscardRequestDto requestDto);
    }
}
