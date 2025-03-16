using System;
using System.Collections.Generic;

namespace RunBase_API.Models;

public partial class Versenyindulas
{
    public int VersenyzoId { get; set; }

    public int VersenyId { get; set; }

    public int Tav { get; set; }

    public string Indulas { get; set; } = null!;

    public string Erkezes { get; set; } = null!;

    public int Rajtszam { get; set; }

    public virtual Versenyek? Verseny { get; set; }
    public virtual Versenyzo? Versenyzo { get; set; }
}
