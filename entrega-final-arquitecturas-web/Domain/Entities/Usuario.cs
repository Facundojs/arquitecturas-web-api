using entrega_final_arquitecturas_web.DAL.Models;
using System.Reflection.Metadata.Ecma335;

namespace entrega_final_arquitecturas_web.Domain.Entities
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }

    }
}
