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
    [Route("api/v1/passcards/favorite")]
    [Authorize(Roles = "admin, user")]

    public class FavoritePasscardController
        : ControllerBase {
        private readonly IFavoritePasscardService service;
        public FavoritePasscardController(IFavoritePasscardService service) {
            this.service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<PasscardOutDto[]>> GetAllAsync() {
            try {
                var outDtos = await service.GetAllAsync();
                return Ok(outDtos);
            } catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("add/{id}")]
        public virtual async Task<ActionResult<PasscardOutDto>> AddAsync(long id) {
            try {
                await service.AddAsync(id);
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


        [HttpPost("remove/{id}")]
        public virtual async Task<ActionResult<PasscardOutDto>> RemoveAsync(long id) {
            try {
                await service.RemoveAsync(id);
                return Ok();
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
    }
}
