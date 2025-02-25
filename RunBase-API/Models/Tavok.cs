using System;
using System.Collections.Generic;

namespace RunBase_API.Models;

public partial class Tavok
{
    public int TavId { get; set; }

    public int Tav { get; set; }

    public virtual ICollection<Versenyek> Versenies { get; set; } = new List<Versenyek>();
}
