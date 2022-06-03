using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/[controller]s")]
    [Authorize(Roles = "admin, user")]
    public class BaseCrudController<TEntity, TOutDto, TCreateDto, TUpdateDto> : ControllerBase {
        protected readonly IBaseCrudService<TEntity, TOutDto, TCreateDto, TUpdateDto> service;
        public BaseCrudController(
            IBaseCrudService<TEntity, TOutDto, TCreateDto, TUpdateDto> service
        ) {
            this.service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<TOutDto[]>> GetAllAsync() {
            try {
                var outDtos = await service.GetAllAsync();
                return Ok(outDtos);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("create")]
        public virtual async Task<ActionResult<TOutDto>> CreateAsync(TCreateDto createDto) {
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
        public virtual async Task<ActionResult<TOutDto>> UpdateAsync(TUpdateDto updateDto) {
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
