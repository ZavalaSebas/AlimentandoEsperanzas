using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class User
{
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Apellido")]
    public string LastName { get; set; } = null!;
    
    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^(?=.*\\d)(?=.*[\\u0021-\\u002b\\u003c-\\u0040])(?=.*[A-Z])(?=.*[a-z])\\S{8,16}$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un dígito y un carácter especial, y que tenga una longitud de entre 8 y 15 caracteres.")]
    [DisplayName("Contraseña")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Número de identificación")]
    public string IdNumber { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Tipo de identifiación")]
    public int IdentificationType { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^[0-9]{8}$", ErrorMessage = "El número de teléfono debe de tener 8 digitos")]
    [DisplayName("Teléfono")]
    public int PhoneNumber { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Fecha")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Rol")]
    public int Role { get; set; }

    public virtual ICollection<Actionlog> Actionlogs { get; set; } = new List<Actionlog>();

    public virtual ICollection<Errorlog> Errorlogs { get; set; } = new List<Errorlog>();

    public virtual Idtype IdentificationTypeNavigation { get; set; } = null!;

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();
}
