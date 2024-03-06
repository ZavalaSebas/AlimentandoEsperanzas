using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Por favor, ingrese una dirección de correo electrónico válida.")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = null!;

    public string IdNumber { get; set; } = null!;

    public int IdentificationType { get; set; }

    public int PhoneNumber { get; set; }

    public DateTime Date { get; set; }

    public int Role { get; set; }

    public virtual ICollection<Actionlog> Actionlogs { get; set; } = new List<Actionlog>();

    public virtual ICollection<Errorlog> Errorlogs { get; set; } = new List<Errorlog>();

    public virtual Idtype IdentificationTypeNavigation { get; set; } = null!;

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();
}
