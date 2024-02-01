using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Item
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int Quantity { get; set; }

    public int Category { get; set; }

    public virtual Itemcategory CategoryNavigation { get; set; } = null!;
}
