using System;
using System.Collections.Generic;

namespace RunBase_API.Models;

public partial class Felhasznalok
{
    public int Id { get; set; }

    public int? VersenyzoId { get; set; }

    public string Nev { get; set; } = null!;

    public string? Jelszo { get; set; }

    public string Tipus { get; set; } = null!;

    public virtual Versenyzo? Versenyzo { get; set; }
}
