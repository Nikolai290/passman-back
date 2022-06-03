using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/directories")]
    [Authorize(Roles = "admin, user")]

    public class DirectoryController : ControllerBase {

        private readonly IDirectoryService service;

        public DirectoryController(IDirectoryService service) {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<DirectoryOutDto[]>> GetAllAsync() {
            try {
                var outDtos = await service.GetAllAsync();
                return Ok(outDtos);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("short")]
        public async Task<ActionResult<DirectoryShortOutDto[]>> GetAllShort() {
            try {
                var outDtos = await service.GetAllShortAsync();
                return Ok(outDtos);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("create")]
        public virtual async Task<ActionResult<DirectoryOutDto>> CreateAsync(DirectoryCreateDto createDto) {
            try {
                var outDto = await service.CreateAsync(createDto);
                return Ok(outDto);
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update")]
        public virtual async Task<ActionResult<DirectoryOutDto>> UpdateAsync(DirectoryUpdateDto updateDto) {
            try {
                var outDto = await service.UpdateAsync(updateDto);
                return Ok(outDto);
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (Exception e) {
                return BadRequest(e.InnerException.Message);
            }
        }

        [HttpPut("move/{id}/{parentId}")]
        public async Task<ActionResult<DirectoryOutDto>> MoveAsync(long id, long parentId) {
            try {
                var outDto = await service.MoveAsync(id, parentId);
                return Ok(outDto);
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public virtual async Task<ActionResult> DeleteAsync(long id) {
            try {
                await service.DeleteAsync(id);
                return Ok();
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
