using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Category1 { get; set; } = null!;

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
