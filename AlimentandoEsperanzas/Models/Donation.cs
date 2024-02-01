using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Donation
{
    public int DonationId { get; set; }

    public int DonorId { get; set; }

    public double Amount { get; set; }

    public int DonationTypeId { get; set; }

    public DateTime Date { get; set; }

    public int PaymentMethodId { get; set; }

    public int CategoryId { get; set; }

    public string? Comments { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Donationtype DonationType { get; set; } = null!;

    public virtual Donor Donor { get; set; } = null!;

    public virtual Paymentmethod PaymentMethod { get; set; } = null!;
}
