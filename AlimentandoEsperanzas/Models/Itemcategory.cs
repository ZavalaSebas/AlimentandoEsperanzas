using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Itemcategory
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
