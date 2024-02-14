using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models;

public partial class Donationtype
{

    [Key]
    public int DonationTypeId { get; set; }

    [Required(ErrorMessage ="Este dato es requerido")]
    public string DonationType1 { get; set; } = null!;

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
