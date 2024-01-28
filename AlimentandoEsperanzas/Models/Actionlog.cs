using System;
using System.Collections.Generic;

namespace AlimentandoEsperanzas.Models;

public partial class Actionlog
{
    public int ActionLogId { get; set; }

    public DateTime Date { get; set; }

    public string Action { get; set; } = null!;

    public string Document { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
