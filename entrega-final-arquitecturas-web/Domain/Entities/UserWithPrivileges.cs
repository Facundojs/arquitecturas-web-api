namespace entrega_final_arquitecturas_web.Domain.Entities
{
    public class UserWithPrivileges
    {
        public string UserName { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public List<PrivilegeEnum> Privileges{ get; set; } = new();

        public bool Has(PrivilegeEnum privilege)
        {
            return this.Privileges.Any(p => p.Name == privilege.Name);
        }
    }
}
