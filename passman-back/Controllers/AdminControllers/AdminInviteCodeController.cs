using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/admin/invite-codes")]
    [Authorize(Roles = "admin")]
    public class AdminInviteCodeController
        : BaseCrudController<InviteCode, InviteCodeOutDto, InviteCodeCreateDto, InviteCodeUpdateDto> {
        private readonly new IInviteCodeService service;
        public AdminInviteCodeController
            (IInviteCodeService service
        ) : base(service) {
            this.service = service;
        }
        public override Task<ActionResult<InviteCodeOutDto[]>> GetAllAsync() {
            return base.GetAllAsync();
        }

        public override Task<ActionResult<InviteCodeOutDto>> CreateAsync(InviteCodeCreateDto createDto) {
            return base.CreateAsync(createDto);
        }

        [HttpPost("enable/{id}")]
        public async Task<ActionResult> EnableAsync(long id) {
            try {
                await service.EnableAsync(id);
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
        public async Task<ActionResult> DisableAsync(long id) {
            try {
                await service.DisableAsync(id);
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

        public override Task<ActionResult<InviteCodeOutDto>> UpdateAsync(InviteCodeUpdateDto updateDto) {
            return base.UpdateAsync(updateDto);
        }

        public override Task<ActionResult> DeleteAsync(long id) {
            return base.DeleteAsync(id);
        }
    }
}
