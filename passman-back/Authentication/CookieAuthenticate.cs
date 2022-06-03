using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace passman_back.Authentication {
    public static class CookieAuthenticate {
        public static async Task Authenticate(UserOutDto dto, HttpContext httpContext) {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Nickname),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, dto.Role)
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
