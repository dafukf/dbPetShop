using System;
using System.Collections.Generic;

namespace Petshop_SAEP.Models;

public partial class Agendamento
{
    public int Codigo { get; set; }

    public string MedicoConsulta { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public int CodigoPet { get; set; }

    public virtual Pet? CodigoPetNavigation { get; set; } = null!;
}
