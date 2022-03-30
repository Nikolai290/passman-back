using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class BaseCrudController<TEntity, TOutDto, TCreateDto, TUpdateDto> : ControllerBase {
        protected readonly IBaseCrudService<TEntity, TOutDto, TCreateDto, TUpdateDto> service;
        public BaseCrudController(
            IBaseCrudService<TEntity, TOutDto, TCreateDto, TUpdateDto> service
            ) {
            this.service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<TOutDto[]>> GetAll() {
            try {
                var outDtos = await service.GetAllAsync();
                return Ok(outDtos);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("create")]
        public virtual async Task<ActionResult<TOutDto>> Create(TCreateDto createDto) {
            try {
                var outDto = await service.CreateAsync(createDto);
                return Ok(outDto);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update/{id}")]
        public virtual async Task<ActionResult<TOutDto>> Update(long id, TUpdateDto updateDto) {
            try {
                var outDto = await service.UpdateAsync(id, updateDto);
                return Ok(outDto);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public virtual async Task<ActionResult> Delete(long id) {
            try {
                await service.DeleteAsync(id);
                return Ok();
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
