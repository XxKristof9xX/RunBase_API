using System;
using System.Collections.Generic;
using Runbase_API.Models;

namespace RunBase_API.Models;

public partial class Versenyek
{
    public int VersenyId { get; set; }

    public string Nev { get; set; } = null!;

    public string Helyszin { get; set; } = null!;

    public DateOnly Datum { get; set; }

    public string Leiras { get; set; } = null!;

    public int MaxLetszam { get; set; }

    public virtual ICollection<Versenyindulas> Versenyindulas { get; set; } = new List<Versenyindulas>();

    public virtual ICollection<Versenytav> Versenytavs { get; set; } = new List<Versenytav>();
}
