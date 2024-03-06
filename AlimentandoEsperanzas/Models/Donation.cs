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
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Método de pago")]
    public int PaymentMethodId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Categoría")]
    public int CategoryId { get; set; }

    [DisplayName("Comentarios")]
    public string? Comments { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Donationtype DonationType { get; set; } = null!;

    public virtual Donor Donor { get; set; } = null!;

    public virtual Paymentmethod PaymentMethod { get; set; } = null!;
}
