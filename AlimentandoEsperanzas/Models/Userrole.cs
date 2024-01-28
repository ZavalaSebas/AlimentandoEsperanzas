using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Userrole
{
    public int UserRolesId { get; set; }

    public int? RoleId { get; set; }

    public int? UserId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}
