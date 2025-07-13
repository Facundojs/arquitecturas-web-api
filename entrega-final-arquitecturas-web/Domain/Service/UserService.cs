using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace entrega_final_arquitecturas_web.Domain.Service
{
    public class UserService(DbCtx dbCtx, ILogger<UserService> logger)
    {
        public async Task<List<UserWithPrivileges>> GetAllUsersWithPrivilegesAsync()
        {
            var users = await dbCtx.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    PrivilegeNames = u.UsersPrivileges.Select(up => up.Privilege.Name).ToList()
                })
                .ToListAsync();

            logger.LogInformation("Users found: {Count}", users.Count);

            var result = users.Select(u => new UserWithPrivileges
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Privileges = u.PrivilegeNames
                    .Select(PrivilegeEnum.FromName)
                    .ToList()
            }).ToList();

            return result;
        }
        public async Task<UserWithPrivileges> GetUserWithPrivilegesByIdAsync(int userId)
        {
            var user = await dbCtx.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    PrivilegeNames = u.UsersPrivileges.Select(up => up.Privilege.Name).ToList()
                })
                .FirstOrDefaultAsync();
            logger.LogInformation("User: {}", user);
            if (user == null)
                throw new Exception("User not found");

            return new UserWithPrivileges
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Privileges = user.PrivilegeNames
                    .Select(PrivilegeEnum.FromName)
                    .ToList()
            };
        }

        public async Task<List<Privilege>> ValidatePrivileges(List<string> privileges) {
            // traer los privilegios de la base

            var privilegiosEnBbdd = await dbCtx.Privileges
                .Where(p => privileges.Contains(p.Name))
                .ToListAsync();

            // Validar si hay privilegios inexistentes
            var nombresEncontrados = privilegiosEnBbdd.Select(p => p.Name).ToList();
                
            var nombresInvalidos = privileges
                    .Except(nombresEncontrados, StringComparer.OrdinalIgnoreCase)
                    .ToList();

            if (nombresInvalidos.Count > 0)
            {
                throw new Exception(String.Join(",", nombresInvalidos));
            }

            return privilegiosEnBbdd;
        }

    }
}
