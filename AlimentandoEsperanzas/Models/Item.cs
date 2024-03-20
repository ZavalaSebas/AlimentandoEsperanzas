using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Item
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Producto")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Cantidad")]
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Categoría")]
    public int Category { get; set; }
    [DisplayName("Categoría")]
    public virtual Itemcategory? CategoryNavigation { get; set; }
}
