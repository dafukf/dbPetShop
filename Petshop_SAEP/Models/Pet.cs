using System;
using System.Collections.Generic;

namespace Petshop_SAEP.Models;

public partial class Pet
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Raca { get; set; } = null!;

    public int CodigoTutor { get; set; }

    public virtual ICollection<Agendamento> Agendamento { get; set; } = new List<Agendamento>();

    public virtual Tutor? CodigoTutorNavigation { get; set; } = null!;
}
