using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Idtype
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Donor> Donors { get; set; } = new List<Donor>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
