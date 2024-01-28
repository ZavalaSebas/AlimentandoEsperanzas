using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string IdNumber { get; set; } = null!;

    public int PhoneNumber { get; set; }

    public DateTime Date { get; set; }

    public int Role { get; set; }

    public virtual ICollection<Actionlog> Actionlogs { get; set; } = new List<Actionlog>();

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();
}
