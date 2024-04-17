using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Donor : IValidatableObject
{
    [Key]
    public int DonorId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Nombre")]
    public string Name { get; set; } 

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Apellido")]
    public string LastName { get; set; } 

    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage = "El formato del correo electrónico no es válido.")]
    [DisplayName("Correo electrónico")]
    public string Email { get; set; } 

    [Required(ErrorMessage = "El número de identificación es obligatorio")]
    [DisplayName("Número de identificación")]
    public string IdNumber { get; set; } 

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Tipo de identificación")]
    public int IdentificationType { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [RegularExpression("^[0-9]{8}$", ErrorMessage = "El número de teléfono debe de tener 8 digitos")]
    [DisplayName("Teléfono")]
    public required string PhoneNumber { get; set; }


    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Fecha")]
    public DateTime Date { get; set; }

    [StringLength(100, ErrorMessage = "Los comentarios no pueden tener más de 100 caracteres.")]
    [DisplayName("Comentarios")]
    public string? Comments { get; set; }

    public virtual ICollection<Donation>? Donations { get; set; } = new List<Donation>();

    [DisplayName("Tipo de identificación")]
    public virtual Idtype? IdentificationTypeNavigation { get; set; } 

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IdNumber != null)
        {
            if (IdentificationType == 1 && IdNumber.Length != 9) //Cédula Física
            {
                yield return new ValidationResult("La cédula física debe de tener 9 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 5 && IdNumber.Length != 10) //Cédula Jurídica
            {
                yield return new ValidationResult("La cédula jurídica debe de tener 10 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 4 && (IdNumber.Length != 11 && IdNumber.Length != 12)) //Cédula Dimex
            {
                yield return new ValidationResult("La cédula dimex debe de tener 11 o 12 digitos", new[] { "IdNumber" });
            }
            else if (IdentificationType == 8 && IdNumber.Length != 10) //Cédula Nite
            {
                yield return new ValidationResult("La cédula nite debe de tener 10 digitos", new[] { "IdNumber" });
            }
        }
    }
}
