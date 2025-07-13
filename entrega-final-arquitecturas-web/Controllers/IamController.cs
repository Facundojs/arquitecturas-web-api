using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.Service;
using entrega_final_arquitecturas_web.Domain.DTO;
using System;
using System.Security.Claims;
using entrega_final_arquitecturas_web.Domain.Entities;

namespace entrega_final_arquitecturas_web.Controllers
{
    [Route("api/iam")]
    [ApiController]
    [AllowAnonymous]
    public class IamController(DbCtx dbCtx, JwtService jwtService, ILogger<IamController> logger, UserService userService) : ControllerBase
    {
        [HttpGet]
        [Route("perfil")]
        public async Task<IActionResult> Perfil()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await userService.GetUserWithPrivilegesByIdAsync(userId);

            return StatusCode(StatusCodes.Status200OK, new UsuarioDTO(user));
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
    }
}