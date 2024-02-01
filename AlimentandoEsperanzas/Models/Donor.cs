using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Donor
{
    public int DonorId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string IdNumber { get; set; } = null!;

    public int IdentificationType { get; set; }

    public int PhoneNumber { get; set; }

    public DateTime Date { get; set; }

    public string? Comments { get; set; }

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public virtual Idtype IdentificationTypeNavigation { get; set; } = null!;
}
