using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Donationtype
{
    public int DonationTypeId { get; set; }

    public string DonationType1 { get; set; } = null!;

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
