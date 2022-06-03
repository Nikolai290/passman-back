using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Authentication;
using passman_back.Business.Dtos;
using passman_back.Business.Dtos.User;
using passman_back.Business.Interfaces.Services;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace passman_back.Controllers {
    [ApiController]
    [Route("api/v1/user")]
    [AllowAnonymous]
    public class UserAnonymousController : ControllerBase {

        private readonly IUserService service;
        public UserAnonymousController(
            IUserService service
        ) {
            this.service = service;
        }

        [HttpGet("invite-check/{code}")]
        public async Task<ActionResult<bool>> CheckInviteCodeAsync(string code) {
            try {
                var isActive = await service.CheckInviteCodeAsync(code);

                return isActive
                    ? Ok("Это действующий код")
                    : NotFound("Код неактивен");
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

        [HttpGet("restore-check-code/{code}")]
        public async Task<ActionResult> CheckRestorePasswordAsync(string code) {
            try {
                await service.CheckRestoreCode(code);
                return Ok("Это действующий код");
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("restore-step-one")]
        public async Task<ActionResult> StartRestorePasswordAsync([FromBody] UserRestorePasswordStepOneDto dto) {
            try {
                await service.StartRestorePasswordAsync(dto.Login);
                return Ok("Письмо с инструкцией для восстановления пароля выслано Вам на почту");
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (SmtpCommandException e) {
                return BadRequest("Не удалось отправить письмо на указанный почтвый ящик. Используйте реальную почту.");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("restore-step-two")]
        public async Task<ActionResult> EndRestorePasswordAsync([FromBody] UserRestorePasswordStepTwoDto restoreDto) {
            try {
                await service.EndRestorePasswordAsync(restoreDto);
                return Ok("Пароль успешно изменён! Произведите вход");
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserOutDto>> CreateAsync(
            [FromBody] UserRegisterDto createDto,
            [FromQuery] string inviteCode
        ) {
            try {
                var outDto = await service.RegisterAsync(createDto, inviteCode);
                await CookieAuthenticate.Authenticate(outDto, HttpContext);
                return Ok(outDto);
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (SmtpCommandException e) {
                return BadRequest(e.Message);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserOutDto>> LoginAsync([FromBody] UserLoginDto loginDto) {
            try {
                var outDto = await service.LoginAsync(loginDto);
                await CookieAuthenticate.Authenticate(outDto, HttpContext);
                return Ok(outDto);
            } catch (ValidationException e) {
                return BadRequest(e.Errors.Count() > 0 ? e.Errors : e.Message);
            } catch (InvalidOperationException e) {
                return NotFound(e.Message);
            } catch (CryptographicException e) {
                return BadRequest("Ключ шифрования не подходит");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
