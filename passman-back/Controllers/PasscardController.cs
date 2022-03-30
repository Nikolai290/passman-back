using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/Passcards")]
    public class PasscardController
        : BaseCrudController<Passcard, PasscardOutDto, PasscardCreateDto, PasscardUpdateDto> {
        public PasscardController(IPasscardService service) : base(service) {
        }
    }
}
