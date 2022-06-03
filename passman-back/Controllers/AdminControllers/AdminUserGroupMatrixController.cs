using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.Enums;
using SearchingLibrary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/admin/user-groups/matrix")]
    [Authorize(Roles = "admin")]
    public class AdminUserGroupMatrixController : ControllerBase {
        private readonly IAdminUserGroupsService service;
        public AdminUserGroupMatrixController(
            IAdminUserGroupsService service
        ) {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<UserGroupMatrixOutDto[]>> GetAllUserGroupsAsync() {
            try {
                var outDtos = await service.GetAllUserGroupMatrixAsync();
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

        [HttpPut("update/{userGroupId}/{directoryId}/{permission}")]
        public async Task<ActionResult> UpdateMatrixAsync(
            long userGroupId,
            long directoryId,
            Permission permission
        ) {
            try {
                await service.MutateUserGroupAsync(userGroupId,directoryId,permission);
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
