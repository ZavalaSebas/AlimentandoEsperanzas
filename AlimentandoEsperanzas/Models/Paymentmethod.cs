using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Paymentmethod
{
    [Key]
    public int PaymentMethodId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    public string PaymentMethod1 { get; set; } = null!;

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
