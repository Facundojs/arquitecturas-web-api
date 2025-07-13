using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.Entities;
using entrega_final_arquitecturas_web.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

namespace entrega_final_arquitecturas_web.Controllers
{
    [Route("api/user")]
    [ApiController]
    [AllowAnonymous]
    public class UserController(DbCtx dbCtx, JwtService jwtService, ILogger<UserController> logger) : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUsers()
        {
            //var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var users = await dbCtx.Users.ToArrayAsync();

            if (users == null) return StatusCode(StatusCodes.Status500InternalServerError);

            var usersDto = new List<Usuario>();

            foreach(var u in users)
            {
                usersDto.Add(new Usuario
                {
                    Email = u.Email,
                    Id = u.Id,
                    Nombre = u.UserName,
                });
            }

            return StatusCode(StatusCodes.Status200OK, users);
        }
    }
}
