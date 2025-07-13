using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.Service;
using entrega_final_arquitecturas_web.Domain.DTO;
using System;
using System.Security.Claims;

namespace entrega_final_arquitecturas_web.Controllers
{
    [Route("api/iam")]
    [ApiController]
    [AllowAnonymous]
    public class IamController(DbCtx dbCtx, JwtService jwtService, ILogger<IamController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("registro")]
        public async Task<IActionResult> Registro(RegistroDTO dto)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await GetUserWithPrivilegesByIdAsync(userId);

            if (user == null || !user.Privileges.Any(p => p == "ADMIN"))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var usuarioExistente = await dbCtx.Users.Where(u => u.Email == dto.Email)
                .FirstOrDefaultAsync();

            if(usuarioExistente != null) return StatusCode(StatusCodes.Status412PreconditionFailed);

            var Salt = jwtService.GenerateSalt();

            var newUser = new User
            {
                PasswordHash = jwtService.EncryptSHA256(dto.Password, Salt),
                UserName = dto.Nombre,
                Email = dto.Email,
                Salt = Salt,
            };

            await dbCtx.Users.AddAsync(newUser);
            await dbCtx.SaveChangesAsync();

            var success = newUser.Id != 0;
            var statusCode = success ? StatusCodes.Status200OK : StatusCodes.Status422UnprocessableEntity;

            return StatusCode(statusCode);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var usuarioEncontrado = await dbCtx.Users
                .Where(u => u.Email == dto.Email)
                .FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status403Forbidden);

            var salt = usuarioEncontrado.Salt;

            var hashedPassword = jwtService.EncryptSHA256(dto.Password, salt);

            if (hashedPassword != usuarioEncontrado.PasswordHash)
                return StatusCode(StatusCodes.Status403Forbidden);

            var token = jwtService.GenerarJWT(usuarioEncontrado);
            var refreshToken = jwtService.GenerarRefreshToken();

            await dbCtx.RefreshTokens
                .Where(token => token.UserId == usuarioEncontrado.Id)
                .ExecuteDeleteAsync();

            await dbCtx.RefreshTokens.AddAsync(
                new RefreshToken 
                {
                    UserId = usuarioEncontrado.Id,
                    Token = refreshToken,
                    Expires = DateTime.Now.AddDays(1),
                }    
            );

            await dbCtx.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { token, refreshToken });
        }

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(RefreshDTO dto)
        {
            var refreshToken = await dbCtx.RefreshTokens.FirstOrDefaultAsync(token => token.Token == dto.Refresh && token.Expires > DateTime.Now);

            if (refreshToken == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var usuario = await dbCtx.Users.Where(user => user.Id == refreshToken.UserId).FirstOrDefaultAsync();

            var newAccessToken = jwtService.GenerarJWT(usuario!);

            var newRefreshToken = jwtService.GenerarRefreshToken();

            await dbCtx.RefreshTokens
                .Where(token => token.UserId == usuario.Id)
                .ExecuteDeleteAsync();

            await dbCtx.RefreshTokens.AddAsync(
                new RefreshToken
                {
                    UserId = usuario.Id,
                    Token = newRefreshToken,
                    Expires = DateTime.Now.AddDays(1),
                }
            );

            await dbCtx.SaveChangesAsync();

            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        private async Task<UserWithPrivilegesDTO?> GetUserWithPrivilegesByIdAsync(int userId)
        {
            var user = await dbCtx.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserWithPrivilegesDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Privileges = u.UsersPrivileges
                        .Select(up => up.Privilege.Name)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return user;
        }

        private class UserWithPrivilegesDTO
        {
            public int Id { get; set; }
            public string UserName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public List<string> Privileges { get; set; } = new();
        }
    }
}