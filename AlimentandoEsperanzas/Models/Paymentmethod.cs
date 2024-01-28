using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Paymentmethod
{
    public int PaymentMethodId { get; set; }

    public string PaymentMethod1 { get; set; } = null!;

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
