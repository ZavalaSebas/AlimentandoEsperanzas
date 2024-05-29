using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Donation
{
    [Key]
    public int DonationId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Donador")]
    public int DonorId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Cantidad")]
    public double Amount { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Tipo de donación")]
    public int DonationTypeId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Fecha")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Método de pago")]
    public int PaymentMethodId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Categoría")]
    public int CategoryId { get; set; }

    [StringLength(100, ErrorMessage = "Los comentarios no pueden tener más de 100 caracteres.")]
    [DisplayName("Comentarios")]
    public string? Comments { get; set; }

    public virtual Category? Category { get; set; } 

    public virtual Donationtype? DonationType { get; set; } 

    public virtual Donor? Donor { get; set; } 

    public virtual Paymentmethod? PaymentMethod { get; set; } 
}
