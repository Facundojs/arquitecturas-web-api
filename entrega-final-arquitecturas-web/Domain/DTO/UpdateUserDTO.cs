namespace entrega_final_arquitecturas_web.Domain.DTO
{
    public class UpdateUserDTO
    {
        public required String Nombre { get; set; }
        public required String Email { get; set; }
        public List<string> Privilegios { get; set; } = new();
    }
}
