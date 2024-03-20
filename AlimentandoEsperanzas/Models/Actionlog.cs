using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AlimentandoEsperanzas.Models;

public partial class Actionlog
{
    public int ActionLogId { get; set; }

    [DisplayName("Fecha")]
    public DateTime Date { get; set; }

    [DisplayName("Acción")]
    public string Action { get; set; } = null!;

    [DisplayName("Documento")]
    public string Document { get; set; } = null!;

    [DisplayName("Usuario")]
    public int? UserId { get; set; }

    [DisplayName("Usuario")]
    public virtual User? User { get; set; }
}
