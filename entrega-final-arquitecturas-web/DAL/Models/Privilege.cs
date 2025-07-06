using System;
using System.Collections.Generic;

namespace entrega_final_arquitecturas_web.DAL.Models;

public partial class Privilege
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<UsersPrivilege> UsersPrivileges { get; set; } = new List<UsersPrivilege>();
}
