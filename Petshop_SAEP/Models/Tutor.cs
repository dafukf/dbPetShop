using System;
using System.Collections.Generic;

namespace Petshop_SAEP.Models;

public partial class Tutor
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Senha { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
