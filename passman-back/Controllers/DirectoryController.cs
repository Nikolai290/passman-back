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
    public class DirectoryController : ControllerBase {


        public DirectoryController() {}

        [HttpGet]
        public ActionResult<DirectoryOutDto[]> GetAll() {
            throw new NotImplementedException();
        }

        [HttpGet("short")]
        public ActionResult<DirectoryShortOutDto[]> GetAllShort() {
            throw new NotImplementedException();
        }


        [HttpPost("create")]
        public ActionResult<DirectoryOutDto> Create(DirectoryCreateDto createDto) {
            throw new NotImplementedException();
        }

        [HttpPut("update/{id}")]
        public ActionResult<DirectoryOutDto> Update(long id, DirectoryUpdateDto updateDto) {
            throw new NotImplementedException();
        }

        [HttpDelete("delete/{id}")]
        public ActionResult<DirectoryOutDto> Delete(long id) {
            throw new NotImplementedException();
        }
    }
}
