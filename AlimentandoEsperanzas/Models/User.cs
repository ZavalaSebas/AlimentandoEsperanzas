using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class User : IValidatableObject
{
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Nombre")]
    public string Name { get; set; } 

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Apellido")]
    public string LastName { get; set; } 
    
    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; } 

    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^(?=.*\\d)(?=.*[\\u0021-\\u002b\\u003c-\\u0040])(?=.*[A-Z])(?=.*[a-z])\\S{8,16}$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un dígito y un carácter especial, y que tenga una longitud de entre 8 y 15 caracteres.")]
    [DisplayName("Contraseña")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [Compare("Password", ErrorMessage = "La contraseña no coincide.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Número de identificación")]
    public string IdNumber { get; set; } 

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

    public virtual ICollection<Actionlog>? Actionlogs { get; set; } = new List<Actionlog>();

    public virtual ICollection<Errorlog>? Errorlogs { get; set; } = new List<Errorlog>();

    [DisplayName("Tipo de identifiación")]
    public virtual Idtype? IdentificationTypeNavigation { get; set; }

    [DisplayName("Rol")]
    public virtual Role? RoleNavigation { get; set; } 

    public virtual ICollection<Userrole>? Userroles { get; set; } = new List<Userrole>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IdNumber != null)
        {
            if (IdentificationType == 5 && IdNumber.Length != 9) //Cédula Física
            {
                yield return new ValidationResult("La cédula física debe de tener 9 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 7 && IdNumber.Length != 10) //Cédula Jurídica
            {
                yield return new ValidationResult("La cédula jurídica debe de tener 10 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 4 && (IdNumber.Length != 11 && IdNumber.Length != 12)) //Cédula Dimex
            {
                yield return new ValidationResult("La cédula dimex debe de tener 11 o 12 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 1 && IdNumber.Length != 10) //Cédula Nite
            {
                yield return new ValidationResult("La cédula nite debe de tener 10 digitos", new[] { "IdNumber" });
            }
        }
    }
}
