using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.Service;
using entrega_final_arquitecturas_web.Domain.DTO;

namespace entrega_final_arquitecturas_web.Controllers
{
    [Route("api/iam")]
    [ApiController]
    [AllowAnonymous]
    public class Iam : ControllerBase
    {
        private readonly DbCtx _dbCtx;
        private readonly JwtService _jwtService;

        public Iam(DbCtx dbCtx, JwtService jwtService)
        {
            _jwtService = jwtService;
            _dbCtx = dbCtx;
        }

        [HttpPost]
        [Route("registro")]
        public async Task<IActionResult> Registro(RegistroDTO dto)
        {
            var usuarioExistente = await _dbCtx.Users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();

            if(usuarioExistente != null) return StatusCode(StatusCodes.Status412PreconditionFailed);

            var Salt = _jwtService.GenerateSalt();

            var user = new User
            {
                PasswordHash = _jwtService.EncryptSHA256(dto.Password, Salt),
                UserName = dto.Nombre,
                Email = dto.Email,
                Salt = Salt,
            };

            await _dbCtx.Users.AddAsync(user);
            await _dbCtx.SaveChangesAsync();

            var success = user.Id != 0;
            var statusCode = success ? StatusCodes.Status200OK : StatusCodes.Status422UnprocessableEntity;

            return StatusCode(statusCode);
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var usuarioEncontrado = await _dbCtx.Users
                .Where(u => u.Email == dto.Email)
                .FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status403Forbidden);

            var salt = usuarioEncontrado.Salt;

            var hashedPassword = _jwtService.EncryptSHA256(dto.Password, salt);

            if (hashedPassword != usuarioEncontrado.PasswordHash)
                return StatusCode(StatusCodes.Status403Forbidden);

            var token = _jwtService.GenerarJWT(usuarioEncontrado);
            var refreshToken = _jwtService.GenerarRefreshToken();

            await _dbCtx.RefreshTokens
                .Where(token => token.UserId == usuarioEncontrado.Id)
                .ExecuteDeleteAsync();

            await _dbCtx.RefreshTokens.AddAsync(
                new RefreshToken 
                {
                    UserId = usuarioEncontrado.Id,
                    Token = refreshToken,
                    Expires = DateTime.Now.AddDays(1),
                }    
            );

            await _dbCtx.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { token, refreshToken });
        }

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(RefreshDTO dto)
        {
            var refreshToken = await _dbCtx.RefreshTokens.FirstOrDefaultAsync(token => token.Token == dto.Refresh && token.Expires > DateTime.Now);

            if (refreshToken == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var usuario = await _dbCtx.Users.Where(user => user.Id == refreshToken.UserId).FirstOrDefaultAsync();

            var newAccessToken = _jwtService.GenerarJWT(usuario!);

            var newRefreshToken = _jwtService.GenerarRefreshToken();

            await _dbCtx.RefreshTokens
                .Where(token => token.UserId == usuario.Id)
                .ExecuteDeleteAsync();

            await _dbCtx.RefreshTokens.AddAsync(
                new RefreshToken
                {
                    UserId = usuario.Id,
                    Token = newRefreshToken,
                    Expires = DateTime.Now.AddDays(1),
                }
            );

            await _dbCtx.SaveChangesAsync();

            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
    }
}
