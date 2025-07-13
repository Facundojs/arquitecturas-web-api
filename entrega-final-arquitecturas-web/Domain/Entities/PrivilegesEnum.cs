namespace entrega_final_arquitecturas_web.Domain.Entities
{
    public class PrivilegeEnum
    {
        public string Name { get; private set; }

        private PrivilegeEnum(string name)
        {
            Name = name;
        }

        public static readonly PrivilegeEnum BOOKS_LIST = new PrivilegeEnum("BOOKS_LIST");
        public static readonly PrivilegeEnum BOOKS_CREATE = new PrivilegeEnum("BOOKS_CREATE");
        public static readonly PrivilegeEnum BOOKS_DELETE = new PrivilegeEnum("BOOKS_DELETE");
        public static readonly PrivilegeEnum BOOKS_UPDATE = new PrivilegeEnum("BOOKS_UPDATE");

        public static readonly PrivilegeEnum USERS_LIST = new PrivilegeEnum("USERS_LIST");
        public static readonly PrivilegeEnum USERS_CREATE = new PrivilegeEnum("USERS_CREATE");
        public static readonly PrivilegeEnum USERS_DELETE = new PrivilegeEnum("USERS_DELETE");
        public static readonly PrivilegeEnum USERS_UPDATE = new PrivilegeEnum("USERS_UPDATE");

        private static readonly List<PrivilegeEnum> All = new()
            {
                BOOKS_LIST,
                BOOKS_CREATE,
                BOOKS_DELETE,
                BOOKS_UPDATE,
                USERS_LIST,
                USERS_CREATE,
                USERS_DELETE,
                USERS_UPDATE,
            };

        public static PrivilegeEnum FromName(string name)
        {
            return All.FirstOrDefault(p => p.Name == name)
                   ?? throw new ArgumentException($"Privilege '{name}' is invalid");
        }
    }

}