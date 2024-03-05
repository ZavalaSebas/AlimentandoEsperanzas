using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Donor : IValidatableObject
{
    [Key]
    public int DonorId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El número de identificación es obligatorio")]
    [StringLength(9, ErrorMessage = "El número de identificación debe tener como máximo 9 dígitos")]
    public string IdNumber { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]

    public int IdentificationType { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^[0-9]{8}$", ErrorMessage = "El número de teléfono debe de tener 8 digitos")]
    public required string PhoneNumber { get; set; }


    [Required(ErrorMessage = "Este dato es requerido")]
    public DateTime Date { get; set; }

    public string? Comments { get; set; }

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public virtual Idtype IdentificationTypeNavigation { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IdNumber != null)
        {
            if (IdentificationType == 1 && IdNumber.Length != 8) //Cédula Física
            {
                yield return new ValidationResult("La cédula física debe de tener 8 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 2 && IdNumber.Length != 10) //Cédula Jurídica
            {
                yield return new ValidationResult("La cédula jurídica debe de tener 10 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 3 && (IdNumber.Length != 11 && IdNumber.Length != 12)) //Cédula Dimex
            {
                yield return new ValidationResult("La cédula dimex debe de tener 11 o 12 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 4 && IdNumber.Length != 10) //Cédula Nite
            {
                yield return new ValidationResult("La cédula nite debe de tener 10 digitos", new[] { "IdNumber" });
            }
        }
    }
}
