using System;
using System.Collections.Generic;

namespace entrega_final_arquitecturas_web.DAL.Models;

public partial class UsersPrivilege
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PrivilegeId { get; set; }

    public virtual Privilege Privilege { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
