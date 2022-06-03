using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Authentication;
using passman_back.Business.Dtos;
using passman_back.Business.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/user")]
    [Authorize(Roles = "admin, user")]

    public class UserController : ControllerBase {
        private readonly IUserService service;
        public UserController(
            IUserService service
        ) {
            this.service = service;
        }

        [HttpGet("logout")]
        public async Task<ActionResult> LogoutAsync() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpGet("current-user")]
        public async Task<ActionResult<UserOutDto>> GetCurrentUserAsync() {
            try {
                var outDto = await service.GetByLoginAsync(User.Identity.Name);
                if (outDto.IsBlocked) {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                } else {
                    await CookieAuthenticate.Authenticate(outDto, HttpContext);
                }
                return Ok(outDto);
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


        [HttpPut("update")]
        public async Task<ActionResult<UserOutDto>> UpdateAsync([FromBody] UserUpdateDto updateDto) {
            try {
                var outDto = await service.UpdateAsync(updateDto);
                await CookieAuthenticate.Authenticate(outDto, HttpContext);
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

        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] UserChangePasswordDto changePasswordDto) {
            try {
                await service.ChangePasswordAsync(changePasswordDto);
                return Ok();
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (SmtpCommandException e) {
                return BadRequest("Не удалось отправить письмо на указанный почтвый ящик.");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
