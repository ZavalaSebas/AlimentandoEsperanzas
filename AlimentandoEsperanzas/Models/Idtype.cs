using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Idtype
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public string Description { get; set; } = null!;

    public virtual ICollection<Donor> Donors { get; set; } = new List<Donor>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
