using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using System;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/Directories")]
    public class DirectoryController
        : BaseCrudController<Directory, DirectoryOutDto, DirectoryCreateDto, DirectoryUpdateDto> {

        private readonly IDirectoryService directoryServise;

        public DirectoryController(IDirectoryService service) : base(service) {
            this.directoryServise = service;
        }

        [HttpGet("short")]
        public async Task<ActionResult<DirectoryShortOutDto[]>> GetAllShort() {
            try {
                var outDtos = await directoryServise.GetAllShortAsync();
                return Ok(outDtos);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
