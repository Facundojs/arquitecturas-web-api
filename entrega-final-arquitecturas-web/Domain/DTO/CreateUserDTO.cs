namespace entrega_final_arquitecturas_web.Domain.DTO
{
    public class CreateUserDTO
    {
        public required String Nombre { get; set; }
        public required String Email { get; set; }
        public required String Password { get; set; }
        public List<string> Privilegios { get; set; } = new();
    }
}
