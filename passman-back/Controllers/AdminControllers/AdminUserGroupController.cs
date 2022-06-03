using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/admin/user-groups")]
    [Authorize(Roles = "admin")]
    public class AdminUserGroupController : ControllerBase {
        private readonly IAdminUserGroupsService service;
        public AdminUserGroupController(
            IAdminUserGroupsService service
        ) {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<UserGroupOutDto[]>> GetAllUserGroupsAsync(string search) {
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

        [HttpPost("create")]
        public async Task<ActionResult<UserGroupOutDto>> CreateUserGroupAsync(UserGroupCreateDto createDto) {
            try {
                var outDtos = await service.CreateAsync(createDto);
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

        [HttpPut("update")]
        public async Task<ActionResult<UserGroupOutDto>> UpdateUserGroupAsync(UserGroupUpdateDto updateDto) {
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
        public async Task<ActionResult> DeleteUserGroupAsync(long id) {
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
