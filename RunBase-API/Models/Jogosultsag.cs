using System;
using System.Collections.Generic;

namespace RunBase_API.Models;

public partial class Jogosultsag
{
    public int FelhasznaloId { get; set; }
    public int VersenyId { get; set; }

    public virtual Felhasznalok Felhasznalo { get; set; } = null!;
    public virtual Versenyek Verseny { get; set; } = null!;
}
