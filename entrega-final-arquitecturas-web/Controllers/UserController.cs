using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.DTO;
using entrega_final_arquitecturas_web.Domain.Entities;
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
    public class UserController(DbCtx dbCtx, JwtService jwtService, ILogger<UserController> logger, UserService userService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var currentUser = await userService.GetUserWithPrivilegesByIdAsync(userId);

            if (!currentUser.Has(PrivilegeEnum.USERS_LIST))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var users = await userService.GetAllUsersWithPrivilegesAsync();

            if (users == null) return StatusCode(StatusCodes.Status500InternalServerError);

            var usersDto = new List<UsuarioDTO>();

            foreach(var user in users)
            {
                usersDto.Add(new UsuarioDTO(user));
            }

            return StatusCode(StatusCodes.Status200OK, usersDto);
        }

        [HttpPost]
        public async Task<IActionResult> Registro(CreateUserDTO dto)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            logger.LogInformation("UserId {}", userId);
            var user = await userService.GetUserWithPrivilegesByIdAsync(userId);

            if (!user.Has(PrivilegeEnum.USERS_CREATE))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var usuarioExistente = await dbCtx.Users
                .Where(u => u.Email == dto.Email)
                .FirstOrDefaultAsync();

            if (usuarioExistente != null)
                return StatusCode(StatusCodes.Status412PreconditionFailed);

            var salt = jwtService.GenerateSalt();

            var newUser = new User
            {
                PasswordHash = jwtService.EncryptSHA256(dto.Password, salt),
                UserName = dto.Nombre,
                Email = dto.Email,
                Salt = salt,
            };

            if (dto.Privilegios.Count == 0)
            {
                return BadRequest(new
                {
                    Message = "Debes asignar por lo menos un privilegio",
                });
            }

            var privilegiosEnBbdd = new List<Privilege>();

            try
            {
                privilegiosEnBbdd = await userService.ValidatePrivileges(dto.Privilegios);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Uno o más privilegios no son válidos.",
                    InvalidPrivileges = e.Message.Split(",")
                });
            }

            await dbCtx.Users.AddAsync(newUser);
            await dbCtx.SaveChangesAsync();

            var usersPrivileges = privilegiosEnBbdd.Select(p => new UsersPrivilege
            {
                UserId = newUser.Id,
                PrivilegeId = p.Id
            });

            await dbCtx.UsersPrivileges.AddRangeAsync(usersPrivileges);
            await dbCtx.SaveChangesAsync();

            var success = newUser.Id != 0;
            var statusCode = success ? StatusCodes.Status200OK : StatusCodes.Status422UnprocessableEntity;

            return StatusCode(statusCode);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var currentUser = await userService.GetUserWithPrivilegesByIdAsync(userId);

            if (!currentUser.Has(PrivilegeEnum.USERS_DELETE))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var privileges = await dbCtx.UsersPrivileges.Where(up => up.UserId == id).ToListAsync();

            foreach (var privilege in privileges)
            {
                dbCtx.UsersPrivileges.Remove(privilege);
            }

            var user = await dbCtx.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            dbCtx.Users.Remove(user);

            await dbCtx.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO dto)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var currentUser = await userService.GetUserWithPrivilegesByIdAsync(userId);

            if (!currentUser.Has(PrivilegeEnum.USERS_UPDATE))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            if (dto.Privilegios.Count == 0)
            {
                return BadRequest(new
                {
                    Message = "Debes asignar por lo menos un privilegio",
                });
            }

            var privilegiosEnBbdd = new List<Privilege>();

            try
            {
                privilegiosEnBbdd = await userService.ValidatePrivileges(dto.Privilegios);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Uno o más privilegios no son válidos.",
                    InvalidPrivileges = e.Message.Split(",")
                });
            }

            var user = await dbCtx.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            if(dto.Email != null)
            {
                user.Email = dto.Email;
            }


            if (dto.Nombre != null)
            {
                user.UserName = dto.Nombre;
            }

            var privileges = await dbCtx.UsersPrivileges.Where(up => up.UserId == id).ToListAsync();

            foreach (var privilege in privileges)
            {
                dbCtx.UsersPrivileges.Remove(privilege);
            }

            var usersPrivileges = privilegiosEnBbdd.Select(p => new UsersPrivilege
            {
                UserId = id,
                PrivilegeId = p.Id
            });

            await dbCtx.UsersPrivileges.AddRangeAsync(usersPrivileges);

            await dbCtx.SaveChangesAsync();

            return Ok();
        }
    }
}
