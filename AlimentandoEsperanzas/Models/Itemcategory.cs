using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Itemcategory
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Categoría")]
    public string Description { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
