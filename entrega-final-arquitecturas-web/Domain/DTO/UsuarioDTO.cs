using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.Entities;
using System.Reflection.Metadata.Ecma335;

namespace entrega_final_arquitecturas_web.Domain.DTO
{
    public class UsuarioDTO
    {
        public UsuarioDTO(UserWithPrivileges user)
        {
            Privilegios = user.Privileges.Select(p => p.Name).ToList();
            Email = user.Email;
            Id = user.Id;
            Nombre = user.UserName;
        }

        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public List<string> Privilegios { get; set; } = new();
    }
}
