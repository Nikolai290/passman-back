using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/admin/users")]
    [Authorize(Roles = "admin")]
    public class AdminUserController : ControllerBase {
        private readonly IAdminUserService service;
        public AdminUserController(
            IAdminUserService service
        ) {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<UserOutDto[]>> GetAllUsersAsync(string search) {
            try {
                var outDtos = await service.GetAllAsync(search);
                return Ok(outDtos);
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

        [HttpGet("short")]
        public async Task<ActionResult<UserShortOutDto[]>> GetAllUsersShortAsync(string search) {
            try {
                var outDtos = await service.GetAllShortAsync(search);
                return Ok(outDtos);
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

        [HttpPost("create")]
        public async Task<ActionResult<UserOutDto>> CreateAsync(UserAdminCreateDto createDto) {
            try {
                var outDtos = await service.CreateAsync(createDto);
                return Ok(outDtos);
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (SmtpCommandException e) {
                return BadRequest("Не удалось отправить письмо на указанный почтвый ящик.");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("enable/{id}")]
        public async Task<ActionResult> EnableUserAsync(long id) {
            try {
                await service.EnableUserAsync(id);
                return Ok();
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

        [HttpPost("disable/{id}")]
        public async Task<ActionResult> DisableUserAsync(long id) {
            try {
                await service.DisableUserAsync(id);
                return Ok();
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

        [HttpPost("drop-password/{id}")]
        public async Task<ActionResult> DropPasswordUserAsync(long id) {
            try {
                await service.DropPasswordUserAsync(id);
                return Ok();
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (SmtpCommandException e) {
                return BadRequest("Не удалось отправить письмо на указанный почтвый ящик.");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<UserOutDto>> UpdateUserAsync(UserAdminUpdateDto updateDto) {
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
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUserAsync(long id) {
            try {
                await service.DeleteAsync(id);
                return Ok();
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
    }
}
