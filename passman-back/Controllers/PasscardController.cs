using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using passman_back.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PasscardController : ControllerBase {


        public PasscardController() {}

        [HttpGet]
        public ActionResult<PasscardOutDto[]> GetAll() {
            throw new NotImplementedException();
        }


        [HttpPost("create")]
        public ActionResult<PasscardOutDto> Create(PasscardCreateDto createDto) {
            throw new NotImplementedException();
        }

        [HttpPut("update/{id}")]
        public ActionResult<PasscardOutDto> Update(long id, PasscardUpdateDto updateDto) {
            throw new NotImplementedException();
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(long id) {
            throw new NotImplementedException();
        }
    }
}
