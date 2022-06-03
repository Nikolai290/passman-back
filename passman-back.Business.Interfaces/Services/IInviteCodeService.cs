using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;
using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IInviteCodeService
        : IBaseCrudService<InviteCode, InviteCodeOutDto, InviteCodeCreateDto, InviteCodeUpdateDto> {
        Task EnableAsync(long id);
        Task DisableAsync(long id);
    }
}
