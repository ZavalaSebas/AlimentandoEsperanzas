using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Role
{
    public int RoleId { get; set; }
    
    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Rol")]
    public string Role1 { get; set; } = null!;

    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
