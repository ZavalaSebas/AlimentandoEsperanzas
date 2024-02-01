using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Errorlog
{
    public int ErrorLogId { get; set; }

    public DateTime Date { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
