using System;
using System.Collections.Generic;

namespace RunBase_API.Models;

public partial class Versenyzo
{
    public int VersenyzoId { get; set; }

    public string Nev { get; set; } = null!;

    public int SzuletesiEv { get; set; }

    public string Neme { get; set; } = null!;

    public string TajSzam { get; set; } = null!;

    public virtual Felhasznalok? Felhasznalok { get; set; }

    public virtual ICollection<Versenyindulas> Versenyindulas { get; set; } = new List<Versenyindulas>();
}
