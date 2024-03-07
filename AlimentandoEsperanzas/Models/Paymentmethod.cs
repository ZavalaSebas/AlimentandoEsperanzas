using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Paymentmethod
{
    [Key]
    public int PaymentMethodId { get; set; }

    [Required(ErrorMessage = "Este dato es requerido")]
    [DisplayName("Método de Pago")]
    public string PaymentMethod1 { get; set; } = null!;

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
