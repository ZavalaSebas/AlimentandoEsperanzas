using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Donor
{
    public int DonorId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]
    [StringLength(12, MinimumLength = 9, ErrorMessage = "El número de identificación es invalido")]
    public string IdNumber { get; set; } = null!;

    [Required(ErrorMessage = "Este dato es requerido")]

    public int IdentificationType { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [StringLength(8)] 
    public int PhoneNumber { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public DateTime Date { get; set; }

    public string? Comments { get; set; }

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public virtual Idtype IdentificationTypeNavigation { get; set; } = null!;
}
