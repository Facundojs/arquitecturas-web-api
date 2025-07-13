using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.DTO;
using entrega_final_arquitecturas_web.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

namespace entrega_final_arquitecturas_web.Controllers
{
    [Route("api/users")]
    [ApiController]
    [AllowAnonymous]
    public class UserController(DbCtx dbCtx, JwtService jwtService, ILogger<UserController> logger, UserService userService) : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetAllUsersWithPrivilegesAsync();

            if (users == null) return StatusCode(StatusCodes.Status500InternalServerError);

            var usersDto = new List<UsuarioDTO>();

            foreach(var user in users)
            {
                usersDto.Add(new UsuarioDTO(user));
            }

            return StatusCode(StatusCodes.Status200OK, usersDto);
        }
    }
}
