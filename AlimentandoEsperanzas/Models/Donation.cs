using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Donation
{
    [Key]
    public int DonationId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public int DonorId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public double Amount { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public int DonationTypeId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public int PaymentMethodId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public int CategoryId { get; set; }

    public string? Comments { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Donationtype DonationType { get; set; } = null!;

    public virtual Donor Donor { get; set; } = null!;

    public virtual Paymentmethod PaymentMethod { get; set; } = null!;
}
